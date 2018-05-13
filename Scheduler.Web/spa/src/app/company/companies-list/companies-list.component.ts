import { Component, OnInit, Inject } from '@angular/core';
import { CompaniesService } from '../companies.service';
import { IListModel } from '../../common/IListModel';
import { ActivatedRoute, Router } from '@angular/router';
//import { clearTimeout } from 'timers';

@Component({
    selector: 'app-companies-list',
    templateUrl: './companies-list.component.html',
    styleUrls: ['./companies-list.component.scss']
})
export class CompaniesListComponent implements OnInit {

    public data: IListModel;

    public filter: any;

    constructor(
        private service: CompaniesService,
        private route: ActivatedRoute,
        private router: Router,
    ) {
        this.filter = { title: null };
        this.data = <IListModel> {};
    }

    ngOnInit() {


        //this.data = { "totalCount": 5, "items": [{ "id": 1, "title": "Company 1", "isApproved": true }, { "id": 2, "title": "Company 2", "isApproved": true }, { "id": 3, "title": "Company 3", "isApproved": true }, { "id": 4, "title": "Company e5beb3b5-d45c-4e1e-9787-27ff213b8508", "isApproved": true }, { "id": 5, "title": "jgfjgj2", "isApproved": false }] };

        this.route.queryParamMap.subscribe((prm) => {
            for (let k of prm.keys) {
                this.filter[k] = prm.get(k);
            }

            this.service.getList(this.filter)
                .subscribe((result: any) => {
                    Object.assign(this.data, result);
                });

        });
        
    }

    public applySearch() {
        this.router
            .navigate([this.route.routeConfig.path], {
                queryParams: this.filter,
                queryParamsHandling: 'merge'
            });
    }

    private searchTimeout: any;
    private prevFltrJson: string;

    public filterChanged() {

        var fltrJson = JSON.stringify(this.filter);

        if (fltrJson == this.prevFltrJson)
            return;

        this.prevFltrJson = fltrJson;

        if (this.searchTimeout) {
            clearTimeout(this.searchTimeout);
            this.searchTimeout = null;
        }

        this.searchTimeout = setTimeout(() => {
            this.searchTimeout = null;
            this.applySearch();
        }, 800);
    }

    public clearTitle() {
        this.filter.title = '';
        this.filterChanged();
    }

}


