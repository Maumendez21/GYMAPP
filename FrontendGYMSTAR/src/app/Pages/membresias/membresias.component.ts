import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { Membresia } from '../../Interfaces/Membresia.interface';
import { MembresiasService } from '../../Services/membresias.service';
import { Store } from '@ngrx/store';
import { AppState } from '../../app.reducer';
import { Subscription, filter } from 'rxjs';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-membresias',
  templateUrl: './membresias.component.html',
  styleUrls: ['./membresias.component.css'],
})
export class MembresiasComponent implements OnInit {
  public listMembresia: Membresia[] = [];

  public isVisible = false;
  public isConfirmLoading = false;
  public loading = false;
  public membresiaSelected: Membresia | null = null;
  public titleModal = '';
  public storeSubs!: Subscription;

  public idGym = 0;
  public idUser = 0;

  constructor(
    private fb: FormBuilder,
    private membresiaService: MembresiasService,
    private store: Store<AppState>,
    private toastrService: ToastrService,
    private modal: NzModalService
  ) {}

  public membresiaForm = this.fb.group({
    memnombre: ['', [Validators.required]],
    memdescripcion: [''],
    memprecio: [0, [Validators.required]],
    memduracion: [0, [Validators.required, Validators.min(1)]],
    mempersonas: [0, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    this.loading = true;
    this.storeSubs = this.store
      .select('auth')
      .pipe(filter((auth) => auth.gym != null))
      .subscribe((data: any) => {
        this.idGym = data.gym.gymid;
        this.idUser = data.user.usrId;
        this.loadListMembresia();
        this.loading = false;
      });
  }

  loadListMembresia() {
    
    this.membresiaService.ListMembresias(this.idGym).subscribe((resp: any) => {
      if (resp.ok) {
        this.listMembresia = resp.data;
        
      }
    });
  }

  showModal(membresia: Membresia | null): void {
    if (membresia === null) {
      this.membresiaSelected = null;
      this.titleModal = 'Registrar Membresia';
    } else {
      this.membresiaSelected = membresia;

      const { memnombre, memdescripcion, memprecio, memduracion, mempersonas } =
        membresia;
      this.membresiaForm.setValue({
        memnombre,
        memdescripcion,
        memprecio,
        memduracion,
        mempersonas,
      });
      this.titleModal = 'Actualizar ' + membresia.memnombre;
    }

    this.isVisible = true;
  }

  handleOk(): void {
    if (!this.membresiaForm.valid) return;

    let membresiaSend: any = {};
    this.isConfirmLoading = true;
    if (this.membresiaSelected) {
      const membresiaTemp: any = {
        ...this.membresiaForm.value,
        memid: this.membresiaSelected.memid,
        gymId: this.idGym,
        memusrReg: this.idUser,
      };
      membresiaSend = membresiaTemp;
    } else {
      const membresiaTemp: any = {
        ...this.membresiaForm.value,
        memid: 0,
        gymId: this.idGym,
        memusrReg: this.idUser,
      };
      membresiaSend = membresiaTemp;
    }

    this.membresiaService.addMembresia(membresiaSend).subscribe(
      (data: any) => {
        if (!data.ok) {
          this.toastrService.error(`${data.msg}`, 'Ups :(');
          this.load();
          return;
        }
        this.toastrService.success(`${data.msg}`, 'Correcto');
        this.load();
        this.loadListMembresia();
      },
      (error) => {
        this.toastrService.error(
          `Hubo un error inesperado ${error.message}`,
          'Ups :('
        );
        this.load();
      }
    );
  }

  desHab(id: number, accion: number) {
    const data = {
      idMem: id,
      accion: accion,
    };
    this.membresiaService.desHab(data).subscribe(
      (resp: any) => {
        if (!resp.ok) {
          this.toastrService.error(` ${resp.msg}`, 'Ups :(');
          return;
        }
        this.toastrService.success(`${resp.msg}`, 'Correcto');
        this.loadListMembresia();
      },
      (error) => {
        this.toastrService.error(
          `Hubo un error inesperado ${error.message}`,
          'Ups :('
        );
      }
    );
  }

  showDeleteConfirm(id: number): void {
    this.modal.confirm({
      nzTitle: 'Estas seguro de realizar esta acción?',
      nzContent: '<b style="color: red;">No lo podrás revertir.</b>',
      nzOkText: 'Si',
      nzOkType: 'text',
      nzOkDanger: true,
      nzOnOk: () => this.desHab(id, 2),
      nzCancelText: 'No',
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  handleCancel(): void {
    this.isVisible = false;
    this.membresiaForm.reset();
    this.membresiaSelected = null;
  }

  load() {
    this.isVisible = false;
    this.isConfirmLoading = false;
    this.membresiaForm.reset();
    this.membresiaSelected = null;
  }
}
