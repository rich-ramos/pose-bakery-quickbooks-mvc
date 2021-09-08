class InvoiceFormStateDecoratorBase {
    constructor(invoiceFormState) {
        this.invoiceFormState = invoiceFormState;
    }

    get State() {
        return this.invoiceFormState.State;
    }

    set State(state) {
        this.invoiceFormState.State = state;
    }
}

export default InvoiceFormStateDecoratorBase;