import { Directive, Input, OnChanges, SimpleChanges, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appRefreshSidebar]',
})
export class RefreshSidebarDirective implements OnChanges {
  @Input() appRefreshSidebar!: number;
  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainerRef: ViewContainerRef
  ) {
    viewContainerRef.createEmbeddedView(templateRef);
  }
  ngOnChanges(changes: SimpleChanges): void {

    if (changes['appRefreshSidebar']) {
      this.viewContainerRef.clear();
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
  }






}
