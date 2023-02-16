import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from '../../app.reducer';
import { filter, Subscription } from 'rxjs';
import { CajaService } from '../../Services/caja.service';
declare var $: any;
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  constructor(
    private router: Router,
    private store: Store<AppState>,
    private cajaService: CajaService
  ) {}

  public storeSubs!: Subscription;
  public nameUser!: string;

  ngOnInit(): void {
    this.storeSubs = this.store
      .select('auth')
      .pipe(filter((auth) => auth.user != null))
      .subscribe((data: any) => {
        // console.log(data.user);
        this.nameUser = data.user.nombreUser;
      });
  }

  logout() {
    this.router.navigateByUrl('/login');
  }

  navigate(ruta: string, side: number ) {
    this.cajaService.actionActive = side;
    this.router.navigateByUrl(ruta);
  }
  opensideBar(id: any) {
    if ($(`.sidebar-menu .dropdown>a.${id}`).parent().hasClass(`active`)) {
      $(`.sidebar-menu .dropdown>a.${id}`)
        .parent()
        .find(`>.sub-menu`)
        .slideUp(`slow`);
      $(`.sidebar-menu .dropdown>a.${id}`).parent().removeClass(`active`);
    } else {
      $(`.sidebar-menu .dropdown>a.${id}`)
        .parent()
        .find(`>.sub-menu`)
        .slideDown(`slow`);
      $(`.sidebar-menu .dropdown>a.${id}`).parent().addClass(`active`);
    }

    return false;
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.storeSubs.unsubscribe();
  }
}
