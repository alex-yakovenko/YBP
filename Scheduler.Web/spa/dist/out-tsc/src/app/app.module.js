"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_1 = require("@angular/platform-browser");
var core_1 = require("@angular/core");
var http_1 = require("@angular/common/http");
var router_1 = require("@angular/router");
var forms_1 = require("@angular/forms");
var ngx_bootstrap_1 = require("ngx-bootstrap");
var ngx_bootstrap_2 = require("ngx-bootstrap");
var app_component_1 = require("./app.component");
var footer_component_1 = require("./layout/footer/footer.component");
var navbar_component_1 = require("./layout/navbar/navbar.component");
var control_sidebar_component_1 = require("./layout/control-sidebar/control-sidebar.component");
var home_component_1 = require("./home/home.component");
var main_sidebar_component_1 = require("./layout/main-sidebar/main-sidebar.component");
var companies_list_component_1 = require("./company/companies-list/companies-list.component");
var company_details_component_1 = require("./company/company-details/company-details.component");
var companies_service_1 = require("./company/companies.service");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            declarations: [
                app_component_1.AppComponent,
                footer_component_1.FooterComponent,
                navbar_component_1.NavbarComponent,
                control_sidebar_component_1.ControlSidebarComponent,
                home_component_1.HomeComponent,
                main_sidebar_component_1.MainSidebarComponent,
                companies_list_component_1.CompaniesListComponent,
                company_details_component_1.CompanyDetailsComponent
            ],
            imports: [
                platform_browser_1.BrowserModule,
                forms_1.FormsModule,
                ngx_bootstrap_1.AlertModule.forRoot(),
                ngx_bootstrap_2.BsDropdownModule.forRoot(),
                http_1.HttpClientModule,
                router_1.RouterModule.forRoot([
                    { path: '', redirectTo: 'home', pathMatch: 'full' },
                    { path: 'home', component: home_component_1.HomeComponent },
                    { path: 'companies', component: companies_list_component_1.CompaniesListComponent },
                    { path: 'company/:id', component: company_details_component_1.CompanyDetailsComponent },
                    { path: '**', redirectTo: 'home' }
                ])
            ],
            providers: [companies_service_1.CompaniesService],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map