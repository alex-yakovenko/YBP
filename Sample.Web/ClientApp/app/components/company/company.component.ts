import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import 'rxjs/add/operator/switchMap';
import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'company',
    templateUrl: './company.component.html'
})
export class CompanyComponent {

    public id: number;

    constructor(
        http: Http,
        @Inject('BASE_URL') baseUrl: string,
        private route: ActivatedRoute
    ) {



    }

/*    ngOnInit() {

        this.route
            .paramMap
            .switchMap((params: ParamMap) => this.edit(params.get('id'))
            );
    }*/

    public edit(id: string|null)
    {
        alert(id);
    }


    public delete(id: number) {
        alert(id);
    }
}


class CompanyInfo {
    id: number;
    title: string;
    isActive: boolean;
}

class CompanyListModel {
    items: CompanyInfo[];
    totalCount: number;
}