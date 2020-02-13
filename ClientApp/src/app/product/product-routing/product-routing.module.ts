import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from '../product-list/product-list.component';
import { ProductDetailsComponent } from '../product-details/product-details.component';
import { ProductUpdateComponent } from '../product-update/product-update.component';
import { ProductDeleteComponent } from 'src/app/product/product-delete/product-delete.component';
import { ProductAddComponent } from '../product-add/product-add.component';

const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'details/:id', component: ProductDetailsComponent },
  { path: 'add', component: ProductAddComponent },
  { path: 'update/:id', component: ProductUpdateComponent },
  { path: 'delete/:id', component: ProductDeleteComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ],
  declarations: []
})
export class ProductRoutingModule { }
