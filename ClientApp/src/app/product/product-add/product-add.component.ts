import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { SuccessDialogComponent } from 'src/app/shared/dialogs/success-dialog/success-dialog.component';
import { ProductService } from 'src/app/shared/services/product.service';
import { Product } from 'src/app/shared/interfaces/product';
import { ConfirmationDialogComponent } from 'src/app/shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { HttpClient } from '@angular/common/http';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.css']
})
export class ProductAddComponent implements OnInit {

  public productForm: FormGroup;
  private dialogConfig;
  private confirmDialogConfig;
  imageSrc: string = '';
  imageLoaded: boolean = false;

  constructor(private location: Location, private productService: ProductService, private dialog: MatDialog,
    private errorService: ErrorHandlerService, private toastService: ToastService) { }

  ngOnInit() {
    this.productForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      code: new FormControl('', [Validators.required, Validators.maxLength(250)]),
      price: new FormControl('', [Validators.required, Validators.min(1), Validators.pattern(/^\d{1,6}(\.\d{1,2})?$/)]),
      photoName: new FormControl('', [Validators.required]),
      photo: new FormControl('', [Validators.required])
      //price: new FormControl('', [Validators.required, Validators.min(1), Validators.pattern(/(?!(^0+(\.0+)?$))^\d{1,3}(\.\d{1,2})?$/)])
      // price: new FormControl('', [Validators.required, Validators.min(1), Validators.pattern(/(?!(^0+(\.0+)?$))^\d{1,3}(\.\d{1,2})?$/), priceMaxValidator]),
    });

    this.dialogConfig = {
      height: '200px',
      width: '300px',
      disableClose: true,
      data: {}
    }

    this.confirmDialogConfig = {
      height: '200px',
      width: '300px',
      data: {}
    };
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.productForm.controls[controlName].hasError(errorName);
  }

  public onCancel = () => {
    this.location.back();
  }

  public createProduct = (formValue) => {

    if (this.productForm.valid) {

      if (formValue.price > 999.00) {
        this.confirmDialogConfig.data = { 'confirmationMessage': 'Are you sure to add price: ' + formValue.price }
        let confirmDialog = this.dialog.open(ConfirmationDialogComponent, this.confirmDialogConfig);

        confirmDialog.afterClosed().subscribe(result => {
          if (result) {
            this.addProduct();
          }
        });
      }
      else {
        this.addProduct();
      }
    }
  }

  getProduct(): Product {
    return { ...this.productForm.value };
  }

  get price() {
    return this.productForm.get('price');
  }

  addProduct() {
    this.productService.addProduct(this.getProduct()).subscribe(result => {
      let dialogRef = this.dialog.open(SuccessDialogComponent, this.dialogConfig);
      dialogRef.afterClosed().subscribe(result => {
        this.location.back();
      });
    },
      (error => {
        this.errorService.dialogConfig = { ...this.dialogConfig };
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
