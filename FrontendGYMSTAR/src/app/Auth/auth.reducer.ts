

import { createReducer, on } from '@ngrx/store';
import { Gym } from '../Models/gym.model';
import { User } from '../Models/user.model';
import { setGym, setUser, unSetUser, unSetGym, unSetCaja, setCaja } from './auth.actions';
import { Caja } from '../Models/caja.model';

export interface State {
  user: User | null;
  gym: Gym | null;
  caja: Caja | null;
}

export const initialState: State = {
  user: null,
  gym: null,
  caja: null,
};

export const _authReducer = createReducer(
  initialState,

  on(setUser, (state, { user }) => ({ ...state, user: { ...user } })),
  on(unSetUser, (state) => ({ ...state, user: null })),
  on(setGym, (state, { gym }) => ({ ...state, gym: { ...gym } })),
  on(unSetGym, (state) => ({ ...state, gym: null })),
  on(setCaja, (state, { caja }) => ({ ...state, caja: { ...caja } })),
  on(unSetCaja, (state) => ({ ...state, caja: null }))
);
