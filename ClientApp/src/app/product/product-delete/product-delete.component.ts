import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { ProductService } from 'src/app/shared/services/product.service';
import { Product } from 'src/app/shared/interfaces/product';
import { SuccessDialogComponent } from 'src/app/shared/dialogs/success-dialog/success-dialog.component';

@Component({
  selector: 'app-product-delete',
  templateUrl: './product-delete.component.html',
  styleUrls: ['./product-delete.component.css']
})
export class ProductDeleteComponent implements OnInit {

  constructor(private location: Location, private productService: ProductService, private dialog: MatDialog,
    private errorService: ErrorHandlerService, private activeRoute: ActivatedRoute) { }

  private dialogConfig;
  public product: Product;

  ngOnInit() {
    this.dialogConfig = {
      height: '200px',
      width: '300px',
      disableClose: true,
      data: {}
    }

    this.getProductById();
  }

  public onCancel = () => {
    this.location.back();
  }

  private getProductById = () => {
    let productId: string = this.activeRoute.snapshot.params['id'];

    this.productService.getProduct(productId)
      .subscribe(res => {
        this.product = res as Product;
      },
        (error) => {
          this.errorService.dialogConfig = this.dialogConfig;
          this.errorService.handleError(error);
        })
  }

  public deleteProduct = () => {

    this.productService.deleteProduct(this.product)
      .subscribe(res => {
        let dialogRef = this.dialog.open(SuccessDialogComponent, this.dialogConfig);

        dialogRef.afterClosed()
          .subscribe(result => {
            this.location.back();
          });
      },
        (error) => {
          this.errorService.dialogConfig = this.dialogConfig;
          this.errorService.handleError(error);
        })
  }
}
