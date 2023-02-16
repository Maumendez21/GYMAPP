import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeAdminComponent } from './home-admin/home-admin.component';
// import { DashboardComponent } from './dashboard/dashboard.component';
// import { ProgressComponent } from './progress/progress.component';
// import { Grafica1Component } from './grafica1/grafica1.component';
// import { AccountSettingsComponent } from './account-settings/account-settings.component';
// import { PromessComponent } from './promess/promess.component';
// import { RxjsComponent } from './rxjs/rxjs.component';
// import { PorfileComponent } from './porfile/porfile.component';
// //Mantenimientos
// import { UsusariosComponent } from './Mantenimientos/ususarios/ususarios.component';
// import { HospitalesComponent } from './Mantenimientos/hospitales/hospitales.component';
// import { MedicosComponent } from './Mantenimientos/medicos/medicos.component';
// import { MedicoComponent } from './Mantenimientos/medicos/medico.component';
// import { BusquedaComponent } from './busqueda/busqueda.component';
// import { AdminGuard } from '../Guards/admin.guard';

const childRoutes: Routes = [
  { path: 'home', component: HomeAdminComponent, data: { title: 'Dashboard' } },
  // { path: 'account-settings', component: AccountSettingsComponent, data: { title: 'Ajustes' } },
  // { path: 'buscar/:termino', component: BusquedaComponent, data: { title: 'Busquedas' } },
  // { path: 'grafica1', component: Grafica1Component, data: { title: 'Grafica 11' } },
  // { path: 'progress', component: ProgressComponent, data: { title: 'Progress Bar' } },
  // { path: 'promesas', component: PromessComponent, data: { title: 'Promesas' } },
  // { path: 'porfile', component: PorfileComponent, data: { title: 'Perfil' } },
  // { path: 'rxjs', component: RxjsComponent, data: { title: 'Rxjs' } },
  // //Mantenimientos
  // { path: 'hospitales', component: HospitalesComponent, data: { title: 'Hospitales' } },
  // { path: 'medico/:id', component: MedicoComponent, data: { title: 'Medicos' } },
  // { path: 'medicos', component: MedicosComponent, data: { title: 'Medicos' } },
  // //rutas de admin
  // { path: 'usuarios', canActivate: [AdminGuard], component: UsusariosComponent, data: { title: 'Usuarios' } },
  // { path: '', redirectTo: '/dashboard', pathMatch: 'full'},
]



@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class ChildRoutesModule { }
