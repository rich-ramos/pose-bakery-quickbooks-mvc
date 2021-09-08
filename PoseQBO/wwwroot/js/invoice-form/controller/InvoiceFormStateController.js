import InvoiceFormState from "../component/InvoiceFormState.js";
import SearchByDateRangeInvoiceFormStateDecorator from "../decorators/SearchByDateRangeInvoiceFormStateDecorator.js";
import SearchByNameAndDateInvoiceFormStateDecorator from "../decorators/SearchByNameAndDateInvoiceFormStateDecorator.js";

class InvoiceFormStateController {
    linkId = 'name-link';

    constructor() {
        this.form = document.querySelector('#invoiceForm');
        this.invoiceNavLinks = document.querySelectorAll('.invoice-nav-link');
        this.InvoiceFormState = new InvoiceFormState();
        this.events();
    }

    events() {
        this.invoiceNavLinks.forEach((invoiceNavLink) => {
            invoiceNavLink.addEventListener('click', (e) => this.handleClick(e));
        });
    }

    handleClick(e) {
        this.invoiceNavLinks.forEach((invoiceNavLink) => {
            invoiceNavLink.classList.remove('active');
        });
        e.target.classList.add('active');
        this.changeFormState(e.target.id);
    }

    changeFormState(id) {
        if (id == this.linkId) {
            const nameAndDateInvoiceFormStateDecorator = new SearchByNameAndDateInvoiceFormStateDecorator(this.InvoiceFormState);
            this.form.innerHTML = nameAndDateInvoiceFormStateDecorator.State;
            this.form.setAttribute('action', 'QBO/InvoicesByNameAndDate');
        } else {
            const dateRangeInvoiceFormStateDecorator = new SearchByDateRangeInvoiceFormStateDecorator(this.InvoiceFormState);
            this.form.innerHTML = dateRangeInvoiceFormStateDecorator.State;
            this.form.setAttribute('action', 'QBO/InvoicesByDateRange');
        }
    }
}

export default InvoiceFormStateController;