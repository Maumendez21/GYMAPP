import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AppState } from '../app.reducer';
import { Store } from '@ngrx/store';
import { User } from '../Models/user.model';
import * as authActions from '../Auth/auth.actions';
import { Router } from '@angular/router';
import { Gym } from '../Models/gym.model';
import { Caja } from '../Models/caja.model';

const base_url = environment.API_URL;
@Injectable({
  providedIn: 'root',
})
export class GeneralService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<AppState>,
  ) {}

  private _user!: User | null;
  private _caja!: Caja | null;
  private _gym!: Gym | null;

  get token(): string {
    return localStorage.getItem('token') || '';
  }

  get role(): 'SADMIN' | 'ADM' | 'CLI' {
    const user = JSON.parse(localStorage.getItem('user') || '');
    return user.rol;
  }

  get headers() {
    return {
      headers: {
        Authorization: 'Bearer ' + this.token,
        'Content-Type': 'application/json',
      },
    };
  }

  login(data: any): Observable<any> {
    return this.http.post<any>(base_url + 'General/Login', data).pipe(
      tap((resp) => {
        if (resp.ok) {
          localStorage.setItem('token', resp.data.token);
          localStorage.setItem('userID', resp.data.usrId);
          localStorage.setItem('gymId', resp.data.gymId);
        } else {
          this._user = null;
          this._gym = null;
        }
      })
    );
  }

  private handleError(error: any) {
    let errMsg = error.message
      ? error.message
      : error.status
      ? `${error.status} - ${error.statusText}`
      : 'Server error';
    return new Error(error);
  }

  validateToken() {
    const url = `${base_url}General/validateToken/${localStorage.getItem(
      'userID'
    )}`;
    return this.http.get(url, { headers: { 'x-token': this.token } }).pipe(
      tap((resp: any) => {
        if (resp.ok) {
          // console.log(resp);
          
          const tempGym: Gym = { ...resp.data.gym };
          const tempUser: User = { ...resp.data.user, Token: this.token };
          this._user = tempUser;
          this._gym = tempGym;
          this.store.dispatch(authActions.setUser({ user: tempUser }));
          this.store.dispatch(authActions.setGym({ gym: tempGym }));
          
          
          if (resp.data.caja !== null) {
            const tempCaja: Caja = { ...resp.data.caja };
            this.store.dispatch(authActions.setCaja({ caja: tempCaja }));
          }


        } else {
          this.logOut();
        }
      })
    );
  }

  isAuth() {
    if (this.token === '') {
      return false;
    }
    return true;
  }

  logOut() {
    this.store.dispatch(authActions.unSetUser());
    this.store.dispatch(authActions.unSetGym());
    this.store.dispatch(authActions.unSetCaja());
    this._user == null;
    this._gym == null;
    this._caja == null;
    localStorage.removeItem('token');
    localStorage.removeItem('userID');
    this.router.navigateByUrl('/login');
  }

  test(): Observable<any> {
    return this.http.get(base_url + 'General');
  }
}
