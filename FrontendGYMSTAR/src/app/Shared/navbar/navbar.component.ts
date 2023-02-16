import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { State, Store } from '@ngrx/store';
import { filter, Subscription } from 'rxjs';
import { GeneralService } from '../../Services/general.service';
import { AppState } from '../../app.reducer';
import { Gym } from '../../Models/gym.model';
import { Caja } from '../../Models/caja.model';
import { CajaService } from '../../Services/caja.service';

declare var $: any;

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  constructor(
    private generalService: GeneralService,
    private store: Store<AppState>,
    private router: Router,
    private cajaService: CajaService
  ) {}

  public storeSubs!: Subscription;
  public nameGym!: string;
  public completoName!: string;
  public primeraLetra!: string;

  public _caja!: Caja;
  public montoActual = 0;

  ngOnInit(): void {
    this.storeSubs = this.store
      .select('auth')
      .pipe(filter((auth) => auth.gym != null))
      .subscribe((data: any) => {
        this._caja = data.caja;

        if (this._caja === null) {
          // console.log('No hay caja');
          // console.log(this._caja);
        } else {
          this.montoActual = data.caja.cajamontoActual;
        }

        this.nameGym = data.gym.gymnombre;
        this.completoName = data.user.nombreCompletoUser;
        this.primeraLetra = this.completoName.charAt(0);
      });
  }

  nuevaVenta(){

    this.cajaService.actionActive = 1;
    this.router.navigateByUrl('/caja/actual');
  }

  opensideBar() {
    document.body.classList.toggle('sidebar-enable');
  }

  logout() {
    this.generalService.logOut();
  }

  collapseSide() {
    $('body').toggleClass('compact-menu');
    $('.sidebar').toggleClass('active');
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.storeSubs.unsubscribe();
  }
}
