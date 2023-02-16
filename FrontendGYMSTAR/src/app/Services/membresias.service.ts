import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Store } from '@ngrx/store';
import { AppState } from '../app.reducer';
import { map } from 'rxjs';
import { Membresia } from '../Interfaces/Membresia.interface';
import { environment } from 'src/environments/environment';
const base_url = environment.API_URL;

@Injectable({
  providedIn: 'root',
})
export class MembresiasService {
  constructor(private http: HttpClient) {}

  get token(): string {
    return localStorage.getItem('token') || '';
  }

  get headers() {
    return {
      headers: {
        Authorization: 'Bearer ' + this.token,
        'Content-Type': 'application/json',
      },
    };
  }

  ListMembresias(idGym: number) {
    const url = `${base_url}Membresia/listMemberships/${idGym}`;
    return this.http.get(url, this.headers).pipe(map((resp) => resp));
  }

  addMembresia(membresia: Membresia) {
    const url = `${base_url}Membresia/addMembership`;
    return this.http.post(url, membresia, this.headers);
  }

  desHab(data: any) {
    const url = `${base_url}Membresia/desHabDelMembership`;
    return this.http.post(url, data, this.headers);
  }

  newSocio(data: any) {
    const url = `${base_url}Asocio/nuevoAsocio`;
    return this.http.post(url, data, this.headers);
  }
  membresiasAsociadas(idGym: number) {
    const url = `${base_url}Asocio/listMembresiasAsocios/${idGym}`;
    return this.http.get(url, this.headers);
  }

  sociosList(idGym: number) {
    const url = `${base_url}Asocio/listSocios/${idGym}`;
    return this.http.get(url, this.headers);
  }
  pagoMembresiaGet(idSocio: number, idMemsocid: number) {
    const url = `${base_url}Asocio/pagoMembresiaGet/${idSocio}/${idMemsocid}`;
    return this.http.get(url, this.headers);
  }
  pagoMembresiaPost(idMemsocid: number) {
    const url = `${base_url}Asocio/pagoMembresiaPost/${idMemsocid}`;
    return this.http.get(url, this.headers);
  }
}
