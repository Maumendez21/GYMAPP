import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthRoutingModule } from './Auth/auth.routing';
import { PagesRoutingModule } from './Pages/pages.routing';
// import { PagesRoutingModule } from './Pages/pages.routing';
// import { AuthRoutingModule } from './auth/auth.routing';

const routes: Routes = [
  { path: '', redirectTo: '/gym/adminDashboard', pathMatch: 'full' },
  { path: '**', redirectTo: '/gym/adminDashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    PagesRoutingModule,
    AuthRoutingModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
