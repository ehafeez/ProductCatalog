import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/shared/interfaces/product';
import { SuccessDialogComponent } from 'src/app/shared/dialogs/success-dialog/success-dialog.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { ProductService } from 'src/app/shared/services/product.service';
import { ConfirmationDialogComponent } from 'src/app/shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.css']
})
export class ProductUpdateComponent implements OnInit {

  public productForm: FormGroup;
  private dialogConfig;
  private confirmDialogConfig;
  public product: Product;
  imageSrc: string = '';
  imageLoaded: boolean = false;

  constructor(private location: Location, private productService: ProductService, private toastService: ToastService,
    private dialog: MatDialog, private errorService: ErrorHandlerService, private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.productForm = new FormGroup({
      code: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      name: new FormControl('', [Validators.required, Validators.maxLength(250)]),
      price: new FormControl('', [Validators.required, Validators.min(1), Validators.pattern(/^\d{1,6}(\.\d{1,2})?$/)]),
      photoName: new FormControl('', [Validators.required]),
      photo: new FormControl('', [Validators.required])
    });

    this.dialogConfig = {
      height: '200px',
      width: '300px',
      disableClose: true,
      data: {}
    }

    this.confirmDialogConfig = {
      height: '250px',
      width: '350px',
      data: {}
    };

    this.getProductById();
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.productForm.controls[controlName].hasError(errorName);
  }

  public onCancel = () => {
    this.location.back();
  }

  get price() {
    return this.productForm.get('price');
  }

  private getProductById = () => {

    let productId: string = this.activeRoute.snapshot.params['id'];
    this.productService.getProduct(productId)
      .subscribe(res => {
        this.product = res as Product;
        this.productForm.patchValue(this.product);
        const ext = this.product.photoName.split('.')[1].toString().toLowerCase();
        this.imageSrc = `data:image/${ext};base64,${this.product.photo}`;
      },
        (error) => {
          this.errorService.dialogConfig = this.dialogConfig;
          this.errorService.handleError(error);
        })
  }

  public updateProduct = (formValue) => {

    if (this.productForm.valid) {

      if (formValue.price > 999.00) {
        this.confirmDialogConfig.data = { 'confirmationMessage': formValue.price }
        let confirmDialog = this.dialog.open(ConfirmationDialogComponent, this.confirmDialogConfig);

        confirmDialog.afterClosed().subscribe(result => {
          if (result) {
            this.updateProductData(formValue);
          }
        });
      } else {
        this.updateProductData(formValue);
      }
    }
  }

  updateProductData(formValue: any) {

    this.product.code = formValue.code;
    this.product.name = formValue.name;
    this.product.price = formValue.price;
    this.product.photoName = formValue.photoName;
    this.product.photo = formValue.photo;

    this.productService.updateProduct(this.product)
      .subscribe(res => {
        let dialogRef = this.dialog.open(SuccessDialogComponent, this.dialogConfig);

        dialogRef.afterClosed()
          .subscribe(result => {
            this.location.back();
          });
      },
        (error => {
          this.errorService.dialogConfig = this.dialogConfig;
          this.errorService.handleError(error);
        })
      )
  }

  handleImageLoad() {
    this.imageLoaded = true;
  }

  photoInputChange(e) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    this.productForm.controls['photoName'].setValue(file ? file.name : '');

    var pattern = /image-*/;
    var reader = new FileReader();

    if (!file.type.match(pattern)) {
      this.notify('invalid format', 'Photo upload')
      return;
    }

    reader.onload = this.previewPhoto.bind(this);
    reader.readAsDataURL(file);
  }

  previewPhoto(e) {
    var reader = e.target;
    this.imageSrc = reader.result;
    this.productForm.patchValue({ photo: reader.result });
  }

  protected notify(message: string, method: string) {
    this.toastService.openSnackBar(message, method);
  }
}
