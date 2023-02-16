import { Component, OnInit } from '@angular/core';
import { CajaService } from '../../Services/caja.service';
import { filter, Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from '../../app.reducer';

@Component({
  selector: 'app-caja-historial',
  templateUrl: './caja-historial.component.html',
  styleUrls: ['./caja-historial.component.css'],
})
export class CajaHistorialComponent implements OnInit {
  constructor(
    private cajasService: CajaService,
    private store: Store<AppState>
  ) {}

  public storeSubs!: Subscription;
  public idGym = 0;
  public msg = '';

  public cajasHist: Array<any> = []

  ngOnInit(): void {
    this.storeSubs = this.store
    .select('auth')
    .pipe(filter((auth) => auth.gym != null))
    .subscribe((data: any) => {
      this.idGym = data.gym.gymid;
      // this.idUser = data.user.usrId;
    });

    this.cargarHistorial();
    
  }


  cargarHistorial(){
    this.cajasService.getCajaHistorial(this.idGym).subscribe((resp: any) => {
      if (!resp.ok) {
        this.msg = resp.msg;
        return;
      }
      this.cajasHist = resp.data;
    });
  }
}
