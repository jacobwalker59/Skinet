import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopComponent } from './shop.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

const routes: Routes = [
  { path: '', component: ShopComponent },
  { path: ':id', component: ProductDetailsComponent , data: {breadcrumb: {alias: 'productDetails'}}}
  // we can use the alias to set the component inside the bread crumb itself
  ,
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShopRoutingModule {}
