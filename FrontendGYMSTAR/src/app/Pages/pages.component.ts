import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GeneralService } from '../Services/general.service';
import { Subscription } from 'rxjs';
import { AppState } from '../app.reducer';
import { Store } from '@ngrx/store';
import * as ui from '../Shared/ui.actions';



@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.css'],
})
export class PagesComponent implements OnInit {
  constructor(
    private generalService: GeneralService,
    private toastrService: ToastrService,
    private store: Store<AppState>
  ) {}

  public initCount = 0;
  cargarSidebar: number = 0;
  public uiSubscription!: Subscription;
  public loading: boolean = false;

  MetisMenu: any;

  ngOnInit(): void {
    this.uiSubscription = this.store
      .select('ui')
      .subscribe((ui) => (this.loading = ui.isLoading));


    this.store.dispatch(ui.isLoading());

    this.generalService.validateToken().subscribe(
      (data) => {
        if (!data.ok) {
          this.toastrService.error('Tu sesión expiró.', 'Adiós');
        }
        this.store.dispatch(ui.stopLoading());
      },
      (err) => {
        this.toastrService.error('Tu sesión expiró.', 'Adiós');
        this.generalService.logOut();
        this.store.dispatch(ui.stopLoading());
      }
    );
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.uiSubscription.unsubscribe();
  }
}
