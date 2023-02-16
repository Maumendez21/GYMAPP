import { ActionReducerMap } from '@ngrx/store';
import * as ui from './Shared/ui.reducer';
import * as auth from './Auth/auth.reducer';

export interface AppState {
  ui: ui.State;
  auth: auth.State;
}

export const appReducers: ActionReducerMap<AppState> = {
  ui: ui._uiReducer,
  auth: auth._authReducer,
};
