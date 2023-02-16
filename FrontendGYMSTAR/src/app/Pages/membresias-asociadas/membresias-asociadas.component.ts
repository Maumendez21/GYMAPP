import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { NzTableLayout, NzTablePaginationPosition, NzTablePaginationType, NzTableSize } from 'ng-zorro-antd/table';
import { MembresiasService } from '../../Services/membresias.service';


interface ItemData {
  name: string;
  age: number | string;
  address: string;
  checked: boolean;
  expand: boolean;
  description: string;
  disabled?: boolean;
}

interface Setting {
  bordered: boolean;
  loading: boolean;
  pagination: boolean;
  sizeChanger: boolean;
  title: boolean;
  header: boolean;
  footer: boolean;
  expandable: boolean;
  checkbox: boolean;
  fixHeader: boolean;
  noResult: boolean;
  ellipsis: boolean;
  simple: boolean;
  size: NzTableSize;
  tableScroll: string;
  tableLayout: NzTableLayout;
  position: NzTablePaginationPosition;
  paginationType: NzTablePaginationType;
}

@Component({
  selector: 'app-membresias-asociadas',
  templateUrl: './membresias-asociadas.component.html',
  styleUrls: ['./membresias-asociadas.component.css'],
})
export class MembresiasAsociadasComponent implements OnInit {
  settingForm?: UntypedFormGroup;
  listOfData: Array<any> = [];
  idGym: any = 0;
  fixedColumn = false;
  scrollX: string | null = null;
  scrollY: string | null = null;
  settingValue!: Setting;

  currentPageDataChange($event: readonly ItemData[]): void {
    // console.log($event);

    // this.displayData = $event;
  }

  getData() {

    this.membresiaService
      .membresiasAsociadas(this.idGym)
      .subscribe((resp: any) => {
        if (!resp.ok) {
          return;
        }

        this.listOfData = resp.data;
        // console.log(this.listOfData);
      });
  }

  constructor(
    private formBuilder: UntypedFormBuilder,
    private membresiaService: MembresiasService
  ) {}

  ngOnInit(): void {
    this.idGym = localStorage.getItem('gymId') || '';

    this.settingForm = this.formBuilder.group({
      bordered: true,
      loading: false,
      pagination: true,
      sizeChanger: true,
      title: true,
      header: true,
      footer: true,
      expandable: true,
      checkbox: true,
      fixHeader: false,
      noResult: false,
      ellipsis: true,
      simple: true,
      size: 'small',
      paginationType: 'small',
      tableScroll: 'unset',
      tableLayout: 'auto',
      position: 'bottom',
    });

    this.settingValue = this.settingForm.value;

    // this.settingForm.get('tableScroll')!.valueChanges.subscribe((scroll) => {
    //   this.fixedColumn = scroll === 'fixed';
    //   this.scrollX = scroll === 'scroll' || scroll === 'fixed' ? '100vw' : null;
    // });
    this.getData();
  }
}
