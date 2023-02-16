import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CajaService } from '../../../Services/caja.service';

@Component({
  selector: 'app-detalle-caja',
  templateUrl: './detalle-caja.component.html',
  styleUrls: ['./detalle-caja.component.css'],
})
export class DetalleCajaComponent implements OnInit {
  constructor(
    private activatedRoute: ActivatedRoute,
    private cajaService: CajaService
  ) {}
  public idCaja = 0;
  public fechaInicio!: Date;
  public fechaFin!: Date;

  isVisibleMiddle = false;
  refe: string = '';

  montoApertura: number = 0;
  montoCierre: number = 0;
  vendido: number = 0;
  

  public pagos: any = [];
  public pagosDetail: any = [];

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((resp: any) => {
      this.idCaja = resp.id;
    });

    this.cajaService.getCajaId(this.idCaja).subscribe((resp: any) => {
      this.fechaInicio = resp.cajafechaApertura;
      this.fechaFin = resp.cajafechaCierre;
      this.montoApertura = resp.cajamontoApertura;
      this.montoCierre = resp.cajamontoCierre;
      this.vendido = this.montoCierre - this.montoApertura;
    });

    this.cajaService.getCajaPagos(this.idCaja).subscribe((resp: any) => {
      this.pagos = resp.data;
    });
  }

  showModalDetail(pagoid: number, ref: string) {
    this.refe = ref;

    this.cajaService.getCajaPagosDetails(pagoid).subscribe((resp: any) => {
      this.pagosDetail = resp.data;
    });

    this.isVisibleMiddle = true;
  }

  handleOkMiddle(): void {
    this.isVisibleMiddle = false;
  }

  handleCancelMiddle(): void {
    this.isVisibleMiddle = false;
  }
}
