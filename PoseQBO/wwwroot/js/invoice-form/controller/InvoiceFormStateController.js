import InvoiceFormState from "../component/InvoiceFormState.js";
import SearchByDateRangeInvoiceFormStateDecorator from "../decorators/SearchByDateRangeInvoiceFormStateDecorator.js";
import SearchByNameInvoiceFormStateDecorator from "../decorators/SearchByNameInvoiceFormStateDecorator.js";

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
            const nameDecorator = new SearchByNameInvoiceFormStateDecorator(this.InvoiceFormState);
            this.form.innerHTML = nameDecorator.State;
        } else {
            const dateDecorator = new SearchByDateRangeInvoiceFormStateDecorator(this.InvoiceFormState);
            this.form.innerHTML = dateDecorator.State;
        }
    }
}

export default InvoiceFormStateController;