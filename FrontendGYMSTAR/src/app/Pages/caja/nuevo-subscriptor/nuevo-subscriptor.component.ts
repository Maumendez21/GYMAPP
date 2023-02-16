import { Component, OnInit, Input } from '@angular/core';
import { CajaService } from '../../../Services/caja.service';
import { MembresiasService } from '../../../Services/membresias.service';
import { FormBuilder, Validators } from '@angular/forms';
import { DetalleVenta } from '../../../Interfaces/detalleVenta.interface';
import { NzModalService } from 'ng-zorro-antd/modal';
import { ToastrService } from 'ngx-toastr';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.reducer';
import { Caja } from '../../../Models/caja.model';
import * as authActions from '../../../Auth/auth.actions';

@Component({
  selector: 'app-nuevo-subscriptor',
  templateUrl: './nuevo-subscriptor.component.html',
  styleUrls: ['./nuevo-subscriptor.component.css'],
})
export class NuevoSubscriptorComponent implements OnInit {
  @Input() idUserGym!: number;
  @Input() idCaja!: number;
  @Input() idUserReg!: number;

  public listMembresias: Array<any> = [];
  public listUserTemp: Array<any> = [];
  public listDetallePago: Array<any> = [];

  public resultData: any = {};

  public membresiaSelected: number = 0;
  public numPersonas: number = 0;
  public membresia: any;

  public total: number = 0;

  public isLoading: boolean = false;

  isVisible2 = false;

  constructor(
    private cajaService: CajaService,
    private membresiaService: MembresiasService,
    private fb: FormBuilder,
    private modal: NzModalService,
    private toastr: ToastrService,
    private store: Store<AppState>
  ) {}

  public userForm = this.fb.group({
    usrnombre: [null, [Validators.required]],
    usrapp: [null, [Validators.required]],
    usrapm: [null],
    usremail: [null, [Validators.required, Validators.email]],
    usrtelefono: [null],
  });

  handleOk2(): void {
    this.isVisible2 = false;
    this.reset();
  }

  ngOnInit(): void {
    this.membresiaService
      .ListMembresias(this.idUserGym)
      .subscribe((resp: any) => {
        // console.log(resp);
        this.listMembresias = resp.data;
      });

    this.userForm.disable();
  }

  submitFormUser() {
    if (!this.userForm.valid) {
      Object.values(this.userForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });

      return;
    }

    const addUser: any = {
      ...this.userForm.value,
    };

    this.listUserTemp.push(addUser);
    this.toastr.success(
      `${this.userForm.value.usrnombre} agregado`,
      'Correcto'
    );
    this.userForm.reset();
  }

  quitardeCuenta(i: number) {
    this.listUserTemp.splice(i, 1);
    this.toastr.success(`Usuario elimado`, 'Correcto');
  }
  showNuevoAsocioConfirm() {
    if (this.listUserTemp.length !== this.numPersonas) {
      this.toastr.warning(
        `Necesitas agregar a ${
          this.numPersonas - this.listUserTemp.length
        } socios`,
        'Atención'
      );
      return;
    }

    this.modal.confirm({
      nzTitle: 'Confirmar Asocio',
      nzOkText: 'Asociar',
      nzOkType: 'primary',
      nzOnOk: () => this.asocioAdd(),
      nzCancelText: 'Cancelar',
      nzOnCancel: () => console.log('Cancel'),
    });
  }
  asocioAdd() {
    this.isLoading = true;

    const dataNewAsocio: any = {
      users: this.listUserTemp,
      memid: this.membresia.memid,
      gymid: this.idUserGym,
    };

    console.log(dataNewAsocio);

    this.membresiaService.newSocio(dataNewAsocio).subscribe(
      (resp: any) => {
        if (!resp.ok) {
          this.toastr.error(`${resp.msg}`, 'Ocurrio algo inesperado');
          return;
        }


        this.resultData.msg = resp.data.details;
        

        // this.toastr.success(`${resp.data.details}`, 'Correcto');

        const addPago = {
          descripcionPago: this.membresia.memnombre,
          usrReg: this.idUserReg,
          conceptoId: 2,
          cajaId: this.idCaja,
          gymId: this.idUserGym,
          detalle: this.listDetallePago,
        };

        this.cajaService.realizaPago(addPago).subscribe(
          (response: any) => {
            if (!response.ok) {
              this.toastr.error(`${response.msg}`, 'Ups');
              return;
            }


            this.resultData.pago = response.msg;
            this.resultData.pagoTotal = response.data.details;

            this.resultData.listUserTemp = this.listUserTemp;
            this.resultData.listDetallePago = this.listDetallePago;
            this.resultData.membresia = this.membresia.memdescripcion;
            console.log(this.resultData);
            
            

            this.cajaService
              .getCaja(this.idUserGym, this.idUserReg)
              .subscribe((caja: any) => {

                const tempCaja: Caja = { ...caja };
                this.store.dispatch(authActions.setCaja({ caja: tempCaja }));
              });

            // this.toastr.success(`${response.msg}`, 'Asocio Realizado');

            this.isVisible2 = true;

            this.reset();
            this.isLoading = false;
          },
          (err) => {
            this.isLoading = false;
            this.toastr.error(`${err.err}`, 'Error');
          }
        );
      },
      (err) => {
        this.toastr.error(`${err.message}`, 'Ocurrio algo inesperado');
        return;
      }
    );
  }

  reset() {
    this.membresia = null;
    this.membresiaSelected = 0;
    this.userForm.reset();
    this.userForm.disable();
    this.numPersonas = 0;
    this.total = 0;

    this.listUserTemp = [];

    this.listDetallePago = [];
  }
  changeSelect() {
    if (this.membresiaSelected === null) {
      this.reset();
      return;
    }

    this.total = 0;
    this.listDetallePago = [];
    this.userForm.enable();
    this.membresia = this.listMembresias.find(
      (mem) => mem.memid == this.membresiaSelected
    );
    this.numPersonas = this.membresia.mempersonas;

    const detalleMem: DetalleVenta = {
      descripcionDetalle: this.membresia.memdescripcion,
      subtotal: this.membresia.memprecio,
      cantidad: 1,
      productId: 0,
    };

    const detalleInscrip: DetalleVenta = {
      descripcionDetalle: 'Inscripción',
      subtotal: 100,
      cantidad: 1,
      productId: 0,
    };

    this.total += detalleMem.subtotal;
    this.total += detalleInscrip.subtotal;

    this.listDetallePago.push(detalleMem);
    this.listDetallePago.push(detalleInscrip);
  }
}
