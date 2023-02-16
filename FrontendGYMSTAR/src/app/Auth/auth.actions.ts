import { createAction, props } from '@ngrx/store';
import { Gym } from '../Models/gym.model';
import { User } from '../Models/user.model';
import { Caja } from '../Models/caja.model';

export const setUser = createAction('[Auth] setUser', props<{ user: User }>());
export const unSetUser = createAction('[Auth] unSetUser');

export const setGym = createAction('[Auth] setGym', props<{ gym: Gym }>());
export const unSetGym = createAction('[Auth] unSetGym');


export const setCaja = createAction('[Auth] setCaja', props<{ caja: Caja }>());
export const unSetCaja = createAction('[Auth] unSetCaja');
