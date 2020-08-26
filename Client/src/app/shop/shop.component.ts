import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IPagination } from '../shared/models/pagination';
import { IProductType } from '../shared/models/productType';
import { IBrand } from '../shared/models/brand';
import {ShopParams} from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  // search input field is a child of shop component
  // we want to access this from shop component.
  products: IProduct[];
  brands: IBrand[];
  types: IProductType[];
  shopParams = new ShopParams();
  totalCount: number;
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price Low To High', value: 'priceAsc'},
    {name: 'Price High To Low', value: 'priceDesc'}
  ];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    // tslint:disable-next-line: no-debugger
    
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.
    getProducts(this.shopParams)
    .subscribe(response =>
    {
      console.log(response);
      // its response .data because remember its wrapped up inside our pagination class
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;
    },
    errors =>
    console.log(errors));
  }

  getBrands() {
    this.shopService.getBrands().subscribe(response =>
    {
      console.log(response);
      // its response .data because remember its wrapped up inside our pagination class
      this.brands = [{id: 0, name: 'All'}, ...response];
      // so what this does above is add the above to the array resonse as the first element.
    },
    errors =>
    console.log(errors));
  }

  getTypes() {
    this.shopService.getProductTypes().subscribe(response =>
    {
      console.log(response);
      // its response .data because remember its wrapped up inside our pagination class
      this.types = [{id: 0, name: 'All'}, ...response];
    },
    errors =>
    console.log(errors));
  }

  onBrandSelected(brandId: number)
  {
    // tslint:disable-next-line: no-debugger
    
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number)
  {
    // tslint:disable-next-line: no-debugger
  
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string)
  {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any) {
      if(this.shopParams.pageNumber !==event){
      this.shopParams.pageNumber = event;
      this.getProducts();
      }
  }

  onSearch(){
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset(){
    this.searchTerm.nativeElement.value = undefined;
    this.shopParams = new ShopParams();
    this.getProducts();
  }

}
