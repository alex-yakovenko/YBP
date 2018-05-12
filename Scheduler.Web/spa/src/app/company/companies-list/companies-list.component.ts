import { Component, OnInit, Inject } from '@angular/core';
import { CompaniesService, ICompanyListItem } from '../companies.service';
import { IListModel } from '../../common/IListModel';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-companies-list',
    templateUrl: './companies-list.component.html',
    styleUrls: ['./companies-list.component.scss']
})
export class CompaniesListComponent implements OnInit {

    public data: IListModel<ICompanyListItem>;

    public filter: CompaniesFilter;

    constructor(
        private service: CompaniesService,
        private route: ActivatedRoute,
        private router: Router,
    ) {
        this.filter = new CompaniesFilter();
    }

    ngOnInit() {

        this.data = { "totalCount": 5, "items": [{ "id": 1, "title": "Company 1", "isApproved": true }, { "id": 2, "title": "Company 2", "isApproved": true }, { "id": 3, "title": "Company 3", "isApproved": true }, { "id": 4, "title": "Company e5beb3b5-d45c-4e1e-9787-27ff213b8508", "isApproved": true }, { "id": 5, "title": "jgfjgj2", "isApproved": false }] };
        
        /*this.service.getList()
            .subscribe(result => {
                this.data = result;
            });*/
    }

    public applySearch() {

        this.router.navigate([this.route.routeConfig.path], { queryParams: this.filter, queryParamsHandling: 'merge' });
    }

}

export class CompaniesFilter {
    public qry: string;
}
