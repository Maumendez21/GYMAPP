import { Component, OnInit, Input } from '@angular/core';
import { CajaService } from '../../../Services/caja.service';

@Component({
  selector: 'app-historial-ventas',
  templateUrl: './historial-ventas.component.html',
  styleUrls: ['./historial-ventas.component.css'],
})
export class HistorialVentasComponent implements OnInit {
  @Input() idUserReg!: number;
  @Input() idUserGym!: number;
  @Input() idCaja!: number;

  public pagos: any = [];
  public pagosDetail: any = [];
  isVisibleMiddle = false;
  refe: string = '';

  constructor(private cajaService: CajaService) {}

  ngOnInit(): void {
    this.cajaService.getCajaPagos(this.idCaja).subscribe((resp: any) => {
      this.pagos = resp.data;
    });
  }

  showModalDetail(pagoid: number, ref: string) {
    this.refe = ref;


    this.cajaService.getCajaPagosDetails(pagoid)
    .subscribe((resp: any) => {
      this.pagosDetail = resp.data;
    })
  


    this.isVisibleMiddle = true;
  }

  handleOkMiddle(): void {
    this.isVisibleMiddle = false;
  }

  handleCancelMiddle(): void {
    this.isVisibleMiddle = false;
  }

  ngOnChanges(): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    this.cajaService.getCajaPagos(this.idCaja).subscribe((resp: any) => {
      this.pagos = resp.data;
    });
  }
}
