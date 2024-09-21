﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternUOW.Core.DTOs.Paymob.PaymobCardDto
{

    public class PaymobCardDto
    {
        public string?type { get; set; }
        public CardObj obj { get; set; }
        public object? issuer_bank { get; set; }
        public string? transaction_processed_callback_responses { get; set; }
    }

    public class CardObj
    {
        public int id { get; set; }
        public bool pending { get; set; }
        public int amount_cents { get; set; }
        public bool success { get; set; }
        public bool is_auth { get; set; }
        public bool is_capture { get; set; }
        public bool is_standalone_payment { get; set; }
        public bool is_voided { get; set; }
        public bool is_refunded { get; set; }
        public bool is_3d_secure { get; set; }
        public int integration_id { get; set; }
        public int profile_id { get; set; }
        public bool has_parent_transaction { get; set; }
        public CardOrder order { get; set; }
        public string? created_at { get; set; }
        public object? transaction_processed_callback_responses { get; set; }
        public string?currency { get; set; }
        public CardSource_Data source_data { get; set; }
        public string?api_source { get; set; }
        public object? terminal_id { get; set; }
        public int merchant_commission { get; set; }
        public object? installment { get; set; }
        public object? discount_details { get; set; }
        public bool is_void { get; set; }
        public bool is_refund { get; set; }
        public CardData1 data { get; set; }
        public bool is_hidden { get; set; }
        public CardPayment_Key_Claims payment_key_claims { get; set; }
        public bool error_occured { get; set; }
        public bool is_live { get; set; }
        public object? other_endpoint_reference { get; set; }
        public int refunded_amount_cents { get; set; }
        public int source_id { get; set; }
        public bool is_captured { get; set; }
        public int captured_amount { get; set; }
        public object? merchant_staff_tag { get; set; }
        public string? updated_at { get; set; }
        public bool is_settled { get; set; }
        public bool bill_balanced { get; set; }
        public bool is_bill { get; set; }
        public int owner { get; set; }
        public object? parent_transaction { get; set; }
    }

    public class CardOrder
    {
        public int id { get; set; }
        public string? created_at { get; set; }
        public bool delivery_needed { get; set; }
        public CardMerchant merchant { get; set; }
        public object? collector { get; set; }
        public int amount_cents { get; set; }
        public CardShipping_Data shipping_data { get; set; }
        public string?currency { get; set; }
        public bool is_payment_locked { get; set; }
        public bool is_return { get; set; }
        public bool is_cancel { get; set; }
        public bool is_returned { get; set; }
        public bool is_canceled { get; set; }
        public object? merchant_order_id { get; set; }
        public object? wallet_notification { get; set; }
        public int paid_amount_cents { get; set; }
        public bool notify_user_with_email { get; set; }
        public CardItem[] items { get; set; }
        public string?order_url { get; set; }
        public int commission_fees { get; set; }
        public int delivery_fees_cents { get; set; }
        public int delivery_vat_cents { get; set; }
        public string?payment_method { get; set; }
        public object? merchant_staff_tag { get; set; }
        public string?api_source { get; set; }
        public CardData data { get; set; }
    }

    public class CardMerchant
    {
        public int id { get; set; }
        public string? created_at { get; set; }
        public string[] phones { get; set; }
        public object? company_emails { get; set; }
        public string?company_name { get; set; }
        public string?state { get; set; }
        public string?country { get; set; }
        public string?city { get; set; }
        public string?postal_code { get; set; }
        public string?street { get; set; }
    }

    public class CardShipping_Data
    {
        public int id { get; set; }
        public string?first_name { get; set; }
        public string?last_name { get; set; }
        public string?street { get; set; }
        public string?building { get; set; }
        public string?floor { get; set; }
        public string?apartment { get; set; }
        public string?city { get; set; }
        public string?state { get; set; }
        public string?country { get; set; }
        public string?email { get; set; }
        public string?phone_number { get; set; }
        public string?postal_code { get; set; }
        public string?extra_description { get; set; }
        public string?shipping_method { get; set; }
        public int order_id { get; set; }
        public int order { get; set; }
    }

    public class CardData
    {
    }

    public class CardItem
    {
        public string?name { get; set; }
        public string?description { get; set; }
        public int amount_cents { get; set; }
        public int quantity { get; set; }
    }

    public class CardSource_Data
    {
        public string?pan { get; set; }
        public string?type { get; set; }
        public object? tenure { get; set; }
        public string?sub_type { get; set; }
    }

    public class CardData1
    {
        public int gateway_integration_pk { get; set; }
        public string?klass { get; set; }
        public string? created_at { get; set; }
        public float amount { get; set; }
        public string?currency { get; set; }
        public CardMigs_Order migs_order { get; set; }
        public string?merchant { get; set; }
        public string?migs_result { get; set; }
        public CardMigs_Transaction migs_transaction { get; set; }
        public string?txn_response_code { get; set; }
        public string?acq_response_code { get; set; }
        public string?message { get; set; }
        public string?merchant_txn_ref { get; set; }
        public string?order_info { get; set; }
        public string?receipt_no { get; set; }
        public string?transaction_no { get; set; }
        public int batch_no { get; set; }
        public string?authorize_id { get; set; }
        public string?card_type { get; set; }
        public string?card_num { get; set; }
        public string?secure_hash { get; set; }
        public string?avs_result_code { get; set; }
        public string?avs_acq_response_code { get; set; }
        public float captured_amount { get; set; }
        public float authorised_amount { get; set; }
        public float refunded_amount { get; set; }
        public string? acs_eci { get; set; }
    }

    public class CardMigs_Order
    {
        public bool acceptPartialAmount { get; set; }
        public float amount { get; set; }
        public string?authenticationStatus { get; set; }
        public CardChargeback chargeback { get; set; }
        public string? creationTime { get; set; }
        public string?currency { get; set; }
        public string?description { get; set; }
        public string?id { get; set; }
        public string? lastUpdatedTime { get; set; }
        public float merchantAmount { get; set; }
        public string?merchantCategoryCode { get; set; }
        public string?merchantCurrency { get; set; }
        public string?status { get; set; }
        public float totalAuthorizedAmount { get; set; }
        public float totalCapturedAmount { get; set; }
        public float totalRefundedAmount { get; set; }
    }

    public class CardChargeback
    {
        public int amount { get; set; }
        public string?currency { get; set; }
    }

    public class CardMigs_Transaction
    {
        public CardAcquirer acquirer { get; set; }
        public float amount { get; set; }
        public string?authenticationStatus { get; set; }
        public string?authorizationCode { get; set; }
        public string?currency { get; set; }
        public string?id { get; set; }
        public string?receipt { get; set; }
        public string?source { get; set; }
        public string?stan { get; set; }
        public string?terminal { get; set; }
        public string?type { get; set; }
    }

    public class CardAcquirer
    {
        public int batch { get; set; }
        public string?date { get; set; }
        public string?id { get; set; }
        public string?merchantId { get; set; }
        public string?settlementDate { get; set; }
        public string?timeZone { get; set; }
        public string?transactionId { get; set; }
    }

    public class CardPayment_Key_Claims
    {
        public int exp { get; set; }
        public CardExtra extra { get; set; }
        public string?pmk_ip { get; set; }
        public int user_id { get; set; }
        public string?currency { get; set; }
        public int order_id { get; set; }
        public int amount_cents { get; set; }
        public CardBilling_Data billing_data { get; set; }
        public int integration_id { get; set; }
        public bool lock_order_when_paid { get; set; }
        public bool single_payment_attempt { get; set; }
    }

    public class CardExtra
    {
    }

    public class CardBilling_Data
    {
        public string?city { get; set; }
        public string?email { get; set; }
        public string?floor { get; set; }
        public string?state { get; set; }
        public string?street { get; set; }
        public string?country { get; set; }
        public string?building { get; set; }
        public string?apartment { get; set; }
        public string?last_name { get; set; }
        public string?first_name { get; set; }
        public string?postal_code { get; set; }
        public string?phone_number { get; set; }
        public string?extra_description { get; set; }
    }

}
