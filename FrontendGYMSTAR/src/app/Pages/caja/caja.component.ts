import { Component, OnInit } from '@angular/core';
import { AppState } from '../../app.reducer';
import { Store } from '@ngrx/store';
import { filter, Subscription, Observable } from 'rxjs';
import { Caja } from '../../Models/caja.model';
import { FormBuilder, Validators } from '@angular/forms';
import { CajaService } from '../../Services/caja.service';
import * as authActions from '../../Auth/auth.actions';
import { ToastrService } from 'ngx-toastr';
import { NzTabsCanDeactivateFn } from 'ng-zorro-antd/tabs';
import { NzModalService } from 'ng-zorro-antd/modal';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-caja',
  templateUrl: './caja.component.html',
  styleUrls: ['./caja.component.css'],
})
export class CajaComponent implements OnInit {
  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private cajaService: CajaService,
    private toastrService: ToastrService,
    private activateRoute: ActivatedRoute,
    private modal: NzModalService
  ) {}

  public storeSubs!: Subscription;

  public _caja!: Caja | null;
  public idGym = 0;
  public idUser = 0;
  public idCaja = 0;

  public actionActive: number = 0;
  public fechaper: string = '';
  public totalCaja: string = '';
  ngOnInit(): void {
    this.actionActive = this.cajaService.action;

    this.storeSubs = this.store
      .select('auth')
      .pipe(filter((auth) => auth.gym != null))
      .subscribe((data: any) => {
        this._caja = data.caja;
        if (this._caja !== null) {
          this.idCaja = data.caja.cajaid;
          let dateIni = new Date(this._caja.cajafechaApertura);
          this.fechaper = dateIni.toLocaleDateString('es-mx', {
            weekday: 'long',
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
          });
          this.totalCaja = this._caja.cajamontoActual;
        }

        this.idGym = data.gym.gymid;
        this.idUser = data.user.usrId;
      });
  }

  public cajaForm = this.fb.group({
    cajamontoApertura: [0, [Validators.required]],
  });

  isVisible = false;
  isOkLoading = false;

  showModal(): void {
    this.isVisible = true;
  }

  handleOk(): void {
    if (!this.cajaForm.valid) {
      console.log('formulario invalido');

      return;
    }
    this.isOkLoading = true;

    const addCaja: any = {
      ...this.cajaForm.value,
      idUserReg: this.idUser,
      idGym: this.idGym,
    };

    this.cajaService.abrirCaja(addCaja).subscribe((resp: any) => {

      if (resp.ok) {
        const tempCaja: Caja = { ...resp.data };
        this.store.dispatch(authActions.setCaja({ caja: tempCaja }));
        this._caja = tempCaja;
        this.toastrService.success(`${resp.msg}`, 'Caja Abierta');
        this.isVisible = false;
        this.isOkLoading = false;
      } else {
        this.toastrService.error(`${resp.msg}`, 'Ups :(');
        this.isOkLoading = false;
      }
    });
  }

  handleCancel(): void {
    this.isVisible = false;

    this.cajaForm.reset();
  }

  showCerrarCaja(): void {
    this.modal.confirm({
      nzTitle: 'Estás seguro de cerrar la caja actual?',
      nzOkText: 'Cerrar',
      nzOkType: 'primary',
      nzOnOk: () => this.cierraCaja(),
      nzCancelText: 'Cancelar',
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  cierraCaja(){

    this.cajaService.cierraCaja(this.idCaja)
    .subscribe((resp: any) => {
      if (!resp.ok) {
        this.toastrService.error(`${resp.msg}`, 'Error');
        return;
      }
      this.toastrService.success(`${resp.data.details}`, 'Caja Cerrada');
      this.store.dispatch(authActions.unSetCaja());
      this._caja = null;
    })

  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.storeSubs.unsubscribe();
  }

}
