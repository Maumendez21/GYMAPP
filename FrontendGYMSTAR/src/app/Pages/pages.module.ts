import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeAdminComponent } from './home-admin/home-admin.component';
import { PagesComponent } from './pages.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { RefreshSidebarDirective } from '../Directive/refresh-sidebar.directive';
import { NgZorroModule } from '../ng-zorro.module';
import { MembresiasComponent } from '../Pages/membresias/membresias.component';
import { MembresiasAsociadasComponent } from '../Pages/membresias-asociadas/membresias-asociadas.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CajaComponent } from '../Pages/caja/caja.component';
import { CajaHistorialComponent } from '../Pages/caja-historial/caja-historial.component';
import { HistorialVentasComponent } from '../Pages/caja/historial-ventas/historial-ventas.component';
import { NuevaVentaComponent } from '../Pages/caja/nueva-venta/nueva-venta.component';
import { NuevoSubscriptorComponent } from '../Pages/caja/nuevo-subscriptor/nuevo-subscriptor.component';
import { PagarSubscripcionComponent } from '../Pages/caja/pagar-subscripcion/pagar-subscripcion.component';
import { DetalleCajaComponent } from '../Pages/caja-historial/detalle-caja/detalle-caja.component';
import { SociosComponent } from '../Pages/socios/socios.component';



@NgModule({
  declarations: [
    HomeAdminComponent,
    PagesComponent,
    RefreshSidebarDirective,
    MembresiasComponent,
    MembresiasAsociadasComponent,
    CajaComponent,
    CajaHistorialComponent,
    HistorialVentasComponent,
    NuevaVentaComponent,
    NuevoSubscriptorComponent,
    PagarSubscripcionComponent,
    DetalleCajaComponent,
    SociosComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    NgZorroModule,
    FormsModule,
    ReactiveFormsModule
  ],
})
export class PagesModule {}
