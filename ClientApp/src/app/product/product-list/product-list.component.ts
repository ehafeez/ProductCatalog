import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { Router } from '@angular/router';
import { Product } from 'src/app/shared/interfaces/product';
import { ProductService } from 'src/app/shared/services/product.service';
import { TableExport } from 'src/app/shared/export/tableExport';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit, AfterViewInit {

  public displayedColumns = ['code', 'name', 'price', 'details', 'update', 'delete'];
  public dataSource = new MatTableDataSource<Product>();

  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  constructor(private productService: ProductService, private errorService: ErrorHandlerService, private router: Router) { }

  ngOnInit() {
    this.getAllProducts();
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  public getAllProducts = () => {
    this.productService.getAllProducts().subscribe(data => {
      if(data !== undefined){
        this.dataSource.data = data;
      }

    },
      (error) => {
        this.errorService.handleError(error);
      });
  }

  public filetProduct = (value: string) => {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  public navigateToDetails = (id: string) => {
    let url: string = `/product/details/${id}`;
    this.router.navigate([url]);
  }

  public navigateToUpdate = (id: string) => {
    let url: string = `/product/update/${id}`;
    this.router.navigate([url]);
  }

  public navigateToDelete = (id: string) => {
    let url: string = `/product/delete/${id}`;
    this.router.navigate([url]);
  }

  exportTable() {
    TableExport.exportToExcel("productCatalog", "products" + new Date().toISOString());
  }

}
