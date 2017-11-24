import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'companieslist',
    templateUrl: './companies.component.html'
})
export class CompaniesComponent {

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

    }
}


