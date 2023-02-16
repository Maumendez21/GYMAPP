import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';
const base_url = environment.API_URL;


@Injectable({
  providedIn: 'root',
})
export class CajaService {
  get token(): string {
    return localStorage.getItem('token') || '';
  }

  public actionActive: number = 0;

  get action(): number {
    return this.actionActive;
  }

  get headers() {
    return {
      headers: {
        Authorization: 'Bearer ' + this.token,
        'Content-Type': 'application/json',
      },
    };
  }

  constructor(private http: HttpClient) {}

  abrirCaja(addCaja: any) {
    const url = `${base_url}Caja/abrirCaja`;
    return this.http.post(url, addCaja, this.headers);
  }
  cierraCaja(idCaja: number) {
    const url = `${base_url}Caja/cierraCaja/${idCaja}`;
    return this.http.get(url, this.headers);
  }
  getCaja(idGym: number, idUser: number) {
    const url = `${base_url}Caja/cajaByIdGym/${idGym}/${idUser}`;
    return this.http.get(url, this.headers);
  }
  getCajaId(idCaja: number) {
    const url = `${base_url}Caja/cajaById/${idCaja}`;
    return this.http.get(url, this.headers);
  }
  
  getCajaPagos(idCaja: number) {
    const url = `${base_url}Caja/cajaPagos/${idCaja}`;
    return this.http.get(url, this.headers);
  }
  getCajaHistorial(idGym: number) {
    const url = `${base_url}Caja/cajaHistorial/${idGym}`;
    return this.http.get(url, this.headers);
  }
  getCajaPagosDetails(idPago: number) {
    const url = `${base_url}Caja/cajaPagosDetail/${idPago}`;
    return this.http.get(url, this.headers);
  }
  
  realizaPago(addPago: any){
    const url = `${base_url}Caja/realizaPago`;
    return this.http.post(url, addPago, this.headers);
  }
}
