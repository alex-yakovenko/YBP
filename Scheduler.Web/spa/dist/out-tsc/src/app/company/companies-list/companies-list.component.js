"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var companies_service_1 = require("../companies.service");
var router_1 = require("@angular/router");
var CompaniesListComponent = /** @class */ (function () {
    function CompaniesListComponent(service, route, router) {
        this.service = service;
        this.route = route;
        this.router = router;
        this.filter = new companies_service_1.CompaniesFilter();
    }
    CompaniesListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.data = { "totalCount": 5, "items": [{ "id": 1, "title": "Company 1", "isApproved": true }, { "id": 2, "title": "Company 2", "isApproved": true }, { "id": 3, "title": "Company 3", "isApproved": true }, { "id": 4, "title": "Company e5beb3b5-d45c-4e1e-9787-27ff213b8508", "isApproved": true }, { "id": 5, "title": "jgfjgj2", "isApproved": false }] };
        this.route.queryParams.subscribe(function (prm) {
            _this.filter = (prm);
            _this.service.getList(_this.filter)
                .subscribe(function (result) {
                _this.data = result;
            });
        });
    };
    CompaniesListComponent.prototype.applySearch = function () {
        this.router.navigate([this.route.routeConfig.path], { queryParams: this.filter, queryParamsHandling: 'merge' });
    };
    CompaniesListComponent = __decorate([
        core_1.Component({
            selector: 'app-companies-list',
            templateUrl: './companies-list.component.html',
            styleUrls: ['./companies-list.component.scss']
        }),
        __metadata("design:paramtypes", [companies_service_1.CompaniesService,
            router_1.ActivatedRoute,
            router_1.Router])
    ], CompaniesListComponent);
    return CompaniesListComponent;
}());
exports.CompaniesListComponent = CompaniesListComponent;
//# sourceMappingURL=companies-list.component.js.map