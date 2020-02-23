import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { SidenavComponent } from './layout/sidenav/sidenav.component';
import { ToolbarComponent } from './layout/toolbar/toolbar.component';
import { HomeComponent } from './layout/home/home.component';
import { ErorrHandlingInterceptor } from './interceptors/error-handling-interceptor';

import { InMemoryDataService } from './database/in-memory-data.service';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';

import { registerLocaleData } from '@angular/common';
import localDE from '@angular/common/locales/nl';

registerLocaleData(localDE);

@NgModule({
  declarations: [
    AppComponent,
    SidenavComponent,
    ToolbarComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    //HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, { dataEncapsulation: false })
  ],
  providers: [{
    provide: LOCALE_ID,
    useValue: 'nl-NL' // 'de-DE' for Germany
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ErorrHandlingInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
