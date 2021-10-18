class InvoiceFormStateDecoratorBase {
    constructor(invoiceFormStateBase) {
        this.invoiceFormStateBase = invoiceFormStateBase;
    }

    get State() {
        return this.invoiceFormStateBase.State;
    }

    set State(state) {
        this.invoiceFormStateBase.State = state;
    }

    get Action() {
        return this.invoiceFormStateBase.Action;
    }

    set Action(action) {
        this.invoiceFormStateBase.Action = action;
    }
}

export default InvoiceFormStateDecoratorBase;