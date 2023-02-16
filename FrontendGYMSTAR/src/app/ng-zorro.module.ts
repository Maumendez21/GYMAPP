import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzListModule } from 'ng-zorro-antd/list';

import { NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NzButtonModule,
    NzTableModule,
    NzModalModule,
    NzFormModule,
    NzAlertModule,
    NzInputModule,
    NzTabsModule,
    NzCollapseModule,
    NzIconModule,
    NzPaginationModule,
    NzAvatarModule,
    NzSelectModule,
    NzDividerModule,
    NzListModule,
    NzAutocompleteModule,
    NzResultModule,
  ],
  exports: [
    NzButtonModule,
    NzTableModule,
    NzModalModule,
    NzFormModule,
    NzAlertModule,
    NzInputModule,
    NzTabsModule,
    NzCollapseModule,
    NzIconModule,
    NzPaginationModule,
    NzAvatarModule,
    NzSelectModule,
    NzDividerModule,
    NzListModule,
    NzAutocompleteModule,
    NzResultModule,
  ],
})
export class NgZorroModule {}
