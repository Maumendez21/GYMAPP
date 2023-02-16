import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { PagesModule } from './Pages/pages.module';
import { AuthModule } from './Auth/auth.module';

//redux
import { StoreModule } from '@ngrx/store';
import { appReducers } from './app.reducer';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from 'src/environments/environment';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import es from '@angular/common/locales/es';
registerLocaleData(es, 'es');




import { NZ_I18N, es_ES } from 'ng-zorro-antd/i18n';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PagesModule,
    BrowserAnimationsModule,
    AuthModule,
    HttpClientModule,
    StoreModule.forRoot(appReducers),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production, // Restrict extension to log-only mode
      autoPause: true, // Pauses recording actions and state changes when the extension window is not open
    }),
    ToastrModule.forRoot({
      maxOpened: 2,
      preventDuplicates: true,
      // positionClass: 'inline',
      // positionClass: 'toast-bottom-center',
      timeOut: 10000,
    }),
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'es' },
    { provide: NZ_I18N, useValue: es_ES },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
