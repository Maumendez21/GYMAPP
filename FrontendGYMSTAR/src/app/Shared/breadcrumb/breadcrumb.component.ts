import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { distinctUntilChanged, filter } from 'rxjs';
import { IBreadCrumb } from '../../Interfaces/breadcrumb.interface';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css'],
})
export class BreadcrumbComponent implements OnInit {
  public breadCrumbs!: Array<IBreadCrumb>;
  public title: string = '';

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.breadCrumbs = this.buildBreadCrumb(this.activatedRoute.root);
      });
  }

  ngOnInit(): void {}

  buildBreadCrumb(
    route: ActivatedRoute,
    url: string = '',
    breadcrumbs: IBreadCrumb[] = []
  ): IBreadCrumb[] {
    //Si no hay routeConfig disponible, estamos en la ruta raíz
    let label =
      route.routeConfig && route.routeConfig.data
        ? route.routeConfig.data['breadcrumb']
        : '';
    let path =
      route.routeConfig && route.routeConfig.data ? route.routeConfig.path : '';

    // Si la ruta es una ruta dinámica como ':id', elimínela
    var lastRoutePart = path?.split('/').pop();
    const isDynamicRoute = lastRoutePart?.startsWith(':');

    if (isDynamicRoute && !!route.snapshot) {
      const paramName = lastRoutePart?.split(':')[1];

      path = path?.replace(
        lastRoutePart || '',
        route.snapshot.params[paramName || '']
      );
      label = route.snapshot.params[paramName || ''];
    }

    //In the routeConfig the complete path is not available,
    //so we rebuild it each time
    const nextUrl = path ? `${url}/${path}` : url;
    this.title = label;

    const breadcrumb: IBreadCrumb = {
      label: label,
      url: nextUrl,
    };

    // Only adding route with non-empty label
    const newBreadcrumbs = breadcrumb.label
      ? [...breadcrumbs, breadcrumb]
      : [...breadcrumbs];
    if (route.firstChild) {
      //If we are not on our current path yet,
      //there will be more children to look after, to build our breadcumb
      return this.buildBreadCrumb(route.firstChild, nextUrl, newBreadcrumbs);
    }
    return newBreadcrumbs;
  }
}
