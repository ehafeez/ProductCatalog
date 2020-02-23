import { Component, OnInit, Output, EventEmitter, LOCALE_ID, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/shared/interfaces/product';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { ProductService } from 'src/app/shared/services/product.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  public product: Product;
  imageLoaded: boolean = false;
  imageSrc: any;
  @Output() selectEmitt = new EventEmitter();

  public onChange = (event) => {
    this.selectEmitt.emit(event.value);
  }

  constructor(private productService: ProductService, private location: Location, @Inject(LOCALE_ID) locale: string,
    private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit() {
    this.getProductDetails();
  }

  public onCancel = () => {
    this.location.back();
  }

  private getProductDetails = () => {
    let id: string = this.activeRoute.snapshot.params['id'];
    this.productService.getProduct(id)
      .subscribe(res => {
        this.product = res as Product;
        const ext = this.product.photoName.split('.')[1].toString().toLowerCase();
        this.imageSrc = `data:image/${ext};base64,${this.product.photo}`;
      },
        (error) => {
          this.errorHandler.handleError(error);
        })
  }

  handleImageLoad() {
    this.imageLoaded = true;
  }
}
