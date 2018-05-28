import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AlertModule } from 'ngx-bootstrap';
import { BsDropdownModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { FooterComponent } from './layout/footer/footer.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { ControlSidebarComponent } from './layout/control-sidebar/control-sidebar.component';
import { HomeComponent } from './home/home.component';
import { MainSidebarComponent } from './layout/main-sidebar/main-sidebar.component';
import { CompaniesListComponent } from './company/companies-list/companies-list.component';
import { CompanyDetailsComponent } from './company/company-details/company-details.component';
import { CompaniesService } from './company/companies.service';
import { PaginationComponent, Pagination } from './layout/pagination/pagination.component';


@NgModule({
    declarations: [
        AppComponent,
        FooterComponent,
        NavbarComponent,
        ControlSidebarComponent,
        HomeComponent,
        MainSidebarComponent,
        CompaniesListComponent,
        CompanyDetailsComponent,
        PaginationComponent        
    ],
    imports: [
        BrowserModule,
        FormsModule,
        AlertModule.forRoot(),
        BsDropdownModule.forRoot(),
        HttpClientModule,

        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'companies', component: CompaniesListComponent },
            { path: 'company/:id', component: CompanyDetailsComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [CompaniesService, Pagination],
    bootstrap: [AppComponent]
})
export class AppModule { }
