import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeAdminComponent } from './home-admin/home-admin.component';
import { PagesComponent } from './pages.component';
import { AuthGuard } from '../Guards/auth.guard';
import { MembresiasComponent } from './membresias/membresias.component';
import { MembresiasAsociadasComponent } from './membresias-asociadas/membresias-asociadas.component';
import { CajaComponent } from './caja/caja.component';
import { CajaHistorialComponent } from './caja-historial/caja-historial.component';
import { DetalleCajaComponent } from './caja-historial/detalle-caja/detalle-caja.component';
import { SociosComponent } from './socios/socios.component';

const routes: Routes = [
  {
    path: 'gym',
    data: { breadcrumb: 'GYM' },
    component: PagesComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    // loadChildren: () => import('./child-routes.module').then(m=>m.ChildRoutesModule)
    children: [
      {
        path: 'adminDashboard',
        component: HomeAdminComponent,
        data: { breadcrumb: 'Dashboard Administrador' },
      },
      { path: '', redirectTo: 'gym/adminDashboard', pathMatch: 'full' },
    ],
  },
  {
    path: 'membresias',
    data: { breadcrumb: 'Membresias' },
    component: PagesComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    children: [
      {
        path: 'registradas',
        component: MembresiasComponent,
        data: { breadcrumb: 'Registradas' },
      },
      {
        path: 'asociadas',
        component: MembresiasAsociadasComponent,
        data: { breadcrumb: 'Asociadas con usuarios' },
      },
    ],
  },
  {
    path: 'socios',
    data: { breadcrumb: 'Socios' },
    component: PagesComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    children: [
      {
        path: 'registrados',
        component: SociosComponent,
        data: { breadcrumb: 'Registrados' },
      },
    ],
  },
  {
    path: 'caja',
    data: { breadcrumb: 'Caja de Venta' },
    component: PagesComponent,
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    children: [
      {
        path: 'actual',
        component: CajaComponent,
        data: { breadcrumb: 'Caja Actual' },
      },
      {
        path: 'historial',
        component: CajaHistorialComponent,
        data: { breadcrumb: 'Historial de cajas' },
      },
      {
        path: 'historial/:id',
        component: DetalleCajaComponent,
        data: { breadcrumb: 'Detalle' },
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {}
