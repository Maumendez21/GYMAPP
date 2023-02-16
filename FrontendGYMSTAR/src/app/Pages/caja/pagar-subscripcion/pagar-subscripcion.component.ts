import { Component, Input, OnInit } from '@angular/core';
import { MembresiasService } from 'src/app/Services/membresias.service';
import { DetalleVenta } from '../../../Interfaces/detalleVenta.interface';
import { NzModalService } from 'ng-zorro-antd/modal';
import { CajaService } from '../../../Services/caja.service';
import { AppState } from '../../../app.reducer';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import * as authActions from '../../../Auth/auth.actions';
import { Caja } from '../../../Models/caja.model';

@Component({
  selector: 'app-pagar-subscripcion',
  templateUrl: './pagar-subscripcion.component.html',
  styleUrls: ['./pagar-subscripcion.component.css'],
})
export class PagarSubscripcionComponent implements OnInit {
  @Input() idUserGym!: number;
  @Input() idCaja!: number;
  @Input() idUserReg!: number;

  selectedValue = 0;

  listSocios: Array<any> = [];
  public dataSocio: any = null;
  public listDetallePago: Array<any> = [];

  public isLoading: boolean = false;

  public resultData: any = {};

  ngOnInit(): void {
    this.getData();
  }
  constructor(
    private membresiaService: MembresiasService,
    private modal: NzModalService,
    private cajaService: CajaService,
    private toastr: ToastrService,
    private store: Store<AppState>
  ) {}

  isVisible = false;
  isVisible2 = false;

  showModal(): void {
    this.listDetallePago = [];
    this.dataSocio = null;
    this.selectedValue = 0;
    this.isVisible = true;
  }

  handleOk(): void {
    if (this.selectedValue == 0 || this.selectedValue == null) {
      return;
    }

    this.membresiaService
      .pagoMembresiaGet(this.selectedValue, 0)
      .subscribe((resp: any) => {
        console.log(resp);
        if (!resp.ok) {
          return;
        }
        const detalleMem: DetalleVenta = {
          descripcionDetalle: resp.data.memdescripcion,
          subtotal: resp.data.precioMembresia,
          cantidad: 1,
          productId: 0,
        };

        this.listDetallePago.push(detalleMem);

        this.isVisible = false;
        this.selectedValue = 0;
        this.dataSocio = resp.data;
      });
  }

  handleOk2(): void {
    this.isVisible2 = false;
    this.selectedValue = 0;
    this.listDetallePago = [];
    this.dataSocio = null;
  }

  handleCancel(): void {
    this.selectedValue = 0;
    this.isVisible = false;
    this.isVisible2 = false;
  }

  getData() {
    this.membresiaService.sociosList(this.idUserGym).subscribe((resp: any) => {
      this.listSocios = resp.data;
    });
  }

  showPagoAsocioConfirm() {
    this.modal.confirm({
      nzTitle: 'Confirmar Pago',
      nzOkText: 'Pagar',
      nzOkType: 'primary',
      nzOnOk: () => this.asocioPago(),
      nzCancelText: 'Cancelar',
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  asocioPago() {
    this.isLoading = true;

    const addPago = {
      descripcionPago: this.dataSocio.memnombre,
      usrReg: this.idUserReg,
      conceptoId: 2,
      cajaId: this.idCaja,
      gymId: this.idUserGym,
      detalle: this.listDetallePago,
    };

    this.cajaService.realizaPago(addPago).subscribe(
      (response: any) => {
        console.log(response);

        this.resultData.pago = response.msg;
        this.resultData.pagoTotal = response.data.details;
        
        if (!response.ok) {
          this.toastr.error(`${response.msg}`, 'Ups');
          return;
        }


        this.membresiaService.pagoMembresiaPost(this.dataSocio.idMemsocid)
        .subscribe((resp: any) => {
          console.log(resp);

          console.log(this.resultData);
          
          
          if (!resp.ok) {
            this.toastr.error(`${response.msg}`, 'Ups');
            return;
          }

          this.resultData.fechaPagoNew = resp.data.fechaPagoNew;
          this.resultData.diasRestantes = resp.data.diasRestantes;
          this.resultData.msg = resp.msg;
          this.resultData.users = resp.data.users;

          this.cajaService
            .getCaja(this.idUserGym, this.idUserReg)
            .subscribe((caja: any) => {
              console.log(caja);

              const tempCaja: Caja = { ...caja };
              this.store.dispatch(authActions.setCaja({ caja: tempCaja }));
            });


            this.isVisible2 = true;






        })

        

        this.toastr.success(`${response.msg}`, 'Asocio Realizado');
        // this.reset();
        this.isLoading = false;
      },
      (err) => {
        this.isLoading = false;
        this.toastr.error(`${err.err}`, 'Error');
      }
    );
  }
}
