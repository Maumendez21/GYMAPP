import { Component, OnInit } from '@angular/core';
import { MembresiasService } from '../../Services/membresias.service';

@Component({
  selector: 'app-socios',
  templateUrl: './socios.component.html',
  styleUrls: ['./socios.component.css'],
})
export class SociosComponent implements OnInit {
  listOfData: Array<any> = [];
  idGym: any = 0;

  constructor(private membresiaService: MembresiasService) {}

  ngOnInit(): void {
    this.idGym = localStorage.getItem('gymId') || '';
    this.getData();
  }

  getData() {
    this.membresiaService.sociosList(this.idGym).subscribe((resp: any) => {
      if (!resp.ok) {
        // mensaje de backend
        return;
      }

      this.listOfData = resp.data;
      console.log(this.listOfData);
    });
  }

  editSocioModal(idUsr: any) {}

  detalleMembresia(idMem: any){

  }
}
