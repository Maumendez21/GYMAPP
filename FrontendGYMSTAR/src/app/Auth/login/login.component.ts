import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { catchError, map, Subscription } from 'rxjs';
import { GeneralService } from 'src/app/Services/general.service';
import { AppState } from '../../app.reducer';
import * as ui from '../../Shared/ui.actions';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  public loading: boolean = false;

  constructor(
    private store: Store<AppState>,
    private generalService: GeneralService,
    private fb: FormBuilder,
    private router: Router,
    private notifyService: ToastrService
  ) {}

  public formLogin!: FormGroup;
  public uiSubscription!: Subscription;

  ngOnInit(): void {
    if (this.generalService.token !== '') {
      this.router.navigateByUrl('/');
    }
    this.formLogin = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });

    this.uiSubscription = this.store
      .select('ui')
      .subscribe((ui) => (this.loading = ui.isLoading));
  }

  login() {
    if (this.formLogin.invalid) {
      return;
    }

    this.store.dispatch(ui.isLoading());

    const data = {
      email: this.formLogin.value.email || '',
      password: this.formLogin.value.password || '',
    };

    this.generalService.login(this.formLogin.value).subscribe(
      (data) => {
        if (!data.ok) {
          this.notifyService.error(`${data.msg}`, 'Ups :(');
          this.store.dispatch(ui.stopLoading());
          return;
        }
        this.store.dispatch(ui.stopLoading());

        this.router.navigateByUrl('/');

        this.notifyService.success(`${data.msg}`, 'Inicio Correcto');
      },
      (error) => {
        this.notifyService.error(
          `Hubo un error inesperado ${error.message}`,
          'Ups :('
        );
        this.store.dispatch(ui.stopLoading());
      }
    );
  }

  cerrar(){
    window.open();

  }

  ngOnDestroy(): void {
    this.uiSubscription.unsubscribe();
  }
}
//
