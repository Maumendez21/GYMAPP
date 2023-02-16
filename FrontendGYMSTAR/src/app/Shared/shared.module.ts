import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { LoadingComponent } from './loading/loading.component';
import { NgZorroModule } from '../ng-zorro.module';

@NgModule({
  declarations: [
    SidebarComponent,
    NavbarComponent,
    BreadcrumbComponent,
    LoadingComponent,
  ],
  imports: [CommonModule, RouterModule, NgZorroModule],
  exports: [
    SidebarComponent,
    NavbarComponent,
    BreadcrumbComponent,
    LoadingComponent,
  ],
})
export class SharedModule {}
