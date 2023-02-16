import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DetalleVenta } from '../../../Interfaces/detalleVenta.interface';
import { ToastrService } from 'ngx-toastr';
import { CajaService } from '../../../Services/caja.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { Subscription, filter } from 'rxjs';
import { AppState } from '../../../app.reducer';
import { Store } from '@ngrx/store';
import { Caja } from '../../../Models/caja.model';

// import * as authActions from ' ../  Auth/auth.actions';
import * as authActions from '../../../Auth/auth.actions';

@Component({
  selector: 'app-nueva-venta',
  templateUrl: './nueva-venta.component.html',
  styleUrls: ['./nueva-venta.component.css'],
})
export class NuevaVentaComponent implements OnInit {
  @Input() idUserReg!: number;
  @Input() idUserGym!: number;
  @Input() idCaja!: number;

  public descripcionPago: string = 'Venta';
  public total: number = 0;
  public listDetails: DetalleVenta[] = [];

  public storeSubs!: Subscription;
  public _caja!: Caja;
  public isLoading: boolean = false;


  public detalleForm = this.fb.group({
    descripcionDetalle: [null, [Validators.required]],
    subtotal: [null, [Validators.required]],
    cantidad: [null, [Validators.required]],
  });

  submitForm(): void {
    if (!this.detalleForm.valid) {
      Object.values(this.detalleForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });

      return;
    }

    const addDetalle: DetalleVenta = {
      ...this.detalleForm.value,
      productId: 0,
    };

    this.total += this.detalleForm.value.subtotal || 0;
    this.listDetails.push(addDetalle);
    this.toastr.success(
      `${this.detalleForm.value.descripcionDetalle} agregado a la cuenta`,
      'Correcto'
    );
    this.detalleForm.reset();
  }

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private cajaService: CajaService,
    private modal: NzModalService,
    private store: Store<AppState>
  ) {}

  ngOnInit(): void {
  }

  quitardeCuenta(i: number) {
    this.total -= this.listDetails[i].subtotal;
    this.listDetails.splice(i, 1);
    this.toastr.success(`Elemento elimiando`, 'Correcto');
  }

  showPagoConfirm(): void {
    this.modal.confirm({
      nzTitle: 'Confirmar el pago',
      nzOkText: 'Pagar',
      nzOkType: 'primary',
      nzOnOk: () => this.realizaPago(),
      nzCancelText: 'Cancelar',
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  realizaPago() {
    this.isLoading = true;
    const addPago = {
      descripcionPago: this.descripcionPago,
      usrReg: this.idUserReg,
      conceptoId: 1,
      cajaId: this.idCaja,
      gymId: this.idUserGym,
      detalle: this.listDetails,
    };

    this.cajaService.realizaPago(addPago).subscribe((response: any) => {
      console.log(response);

      if (!response.ok) {
        this.toastr.error(`${response.msg}`, 'Ups');
        return;
      }
    
      this.cajaService.getCaja(this.idUserGym, this.idUserReg)
      .subscribe((caja: any) => {
        console.log(caja);
        
        const tempCaja: Caja = {...caja}
        this.store.dispatch(authActions.setCaja({ caja: tempCaja }));
      })

      this.toastr.success(`${response.msg}`, 'Pago realizado');
      this.listDetails = [];
      this.total = 0;
      this.descripcionPago = 'Venta';
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
      this.toastr.error(`${err.err}`, 'Error');
    });
  }
}
