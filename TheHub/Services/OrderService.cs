﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TheHub.Errors;
using TheHub.Models;
using System.Data;
using System.Web.Http;
using System.Configuration;

namespace TheHub.Services
{
    public class OrderService : IOrderService
    {


        /// <summary>
        ///     Adds order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public OrderResponse Add(Order order)
        {
            string sOrderNum;
            try
            {
                DataTable dtHeader = CreateDataTableForHeader(order);
                DataTable dtLineItems = CreateDataTableForLineItems(order);
                sOrderNum = PCT_EpicorDMT.PCT.Add_SalesforceOrder(dtHeader, dtLineItems);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(ResponseUtility.CreateHttpResponseMessage(new List<string>() { "Something went wrong: " + ex.Message }, System.Net.HttpStatusCode.InternalServerError));
            }

            // If PCT_EpicorDMT.PCT.Add_SalesforceOrder is successful, it will return the Order Number.
            if (int.TryParse(sOrderNum, out int iOrderNum))
            {
                // Initialize Order response.
                OrderResponse orderResponse = new OrderResponse();
                orderResponse.Messages = new List<ErrorMessage>();
                orderResponse.EpicorOrderNumber = sOrderNum;
                return orderResponse;
            }
            else
            {
                // If sOrderNum is not numeric then it means that Epicor returned an error
                // Split the returned error messages into array
                string[] parsedErrorMessages = sOrderNum.Split('|');
                throw new HttpResponseException(ResponseUtility.CreateHttpResponseMessage(new List<string>(parsedErrorMessages), System.Net.HttpStatusCode.BadRequest));
            }

        }


        private DataTable CreateDataTableForHeader(Order order)
        {
            DataTable dtHeader = new DataTable();
            dtHeader.Columns.Add("CustID", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("AccountID", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("SF_OrderNum", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("PO", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ExpectedDeliveryDate", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("SubscriptionStartDate", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ShippingMethod", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("WarrantyTerm", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ST_Name", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ST_Street", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ST_City", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ST_State", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ST_PostalCode", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("ST_Country", System.Type.GetType("System.String"));
            dtHeader.Columns.Add("OrderSubscriptionTerm", System.Type.GetType("System.Int32"));
            dtHeader.Columns.Add("MonthlySubscription", System.Type.GetType("System.Decimal"));
            dtHeader.Columns.Add("PctSalesRep", System.Type.GetType("System.String"));

            DataRow dataRow = dtHeader.NewRow();
            dataRow["CustID"] = order.EpicorAccountId;
            dataRow["AccountID"] = order.AccountId;
            dataRow["SF_OrderNum"] = order.OrderNumber;
            dataRow["PO"] = order.CustomerPurchaseOrder;
            dataRow["ExpectedDeliveryDate"] = order.ExpectedDeliveryDate;
            dataRow["SubscriptionStartDate"] = order.SubscriptionStartDate;
            dataRow["ShippingMethod"] = order.ShippingMethod;
            dataRow["WarrantyTerm"] = order.WarrantyTerm;
            dataRow["ST_Street"] = order.ShippingStreet;
            dataRow["ST_City"] = order.ShippingCity;
            dataRow["ST_State"] = order.ShippingState;
            dataRow["ST_PostalCode"] = order.ShippingPostalCode;
            dataRow["ST_Country"] = order.ShippingCountry;
            dataRow["ST_Name"] = order.ShipToAttention;
            dataRow["OrderSubscriptionTerm"] = order.OrderSubscriptionTerm;
            dataRow["MonthlySubscription"] = order.MonthlySubscription;
            dataRow["PctSalesRep"] = order.PctSalesRep;
            dtHeader.Rows.Add(dataRow);

            return dtHeader;
        }

        private DataTable CreateDataTableForLineItems(Order order)
        {
            DataTable dtLineItems = new DataTable();

            dtLineItems.Columns.Add("PartNum", System.Type.GetType("System.String"));
            dtLineItems.Columns.Add("OrderQty", System.Type.GetType("System.Int32"));
            dtLineItems.Columns.Add("UnitPrice", System.Type.GetType("System.Double"));
            dtLineItems.Columns.Add("Sub_UnitPrice", System.Type.GetType("System.Double"));
            dtLineItems.Columns.Add("POLine", System.Type.GetType("System.String"));

            if (order.LineItems != null)
            {
                for (int i = 0; i < order.LineItems.Count; i++)
                {
                    DataRow lineItemDataRow = dtLineItems.NewRow();
                    lineItemDataRow["PartNum"] = order.LineItems[i].ProductCode;
                    lineItemDataRow["OrderQty"] = order.LineItems[i].QuantityOrdered.GetValueOrDefault(0);
                    lineItemDataRow["UnitPrice"] = order.LineItems[i].UnitPrice.GetValueOrDefault(0);
                    lineItemDataRow["Sub_UnitPrice"] = 0f;
                    lineItemDataRow["POLine"] = order.LineItems[i].OrderItemNumber;
                    dtLineItems.Rows.Add(lineItemDataRow);
                }
            }
            return dtLineItems;
        }


        /// <summary>
        ///     Get Shipment Details by salesforceOrderNumber
        /// </summary>
        /// <param name="salesforceOrderNumber"></param>
        /// <returns></returns>
        public Shipment GetShipmentDetails(string salesforceOrderNumber)
        {
            var shipment = new Shipment();
            int rowCount;
            try
            {
                DataTable dt = new DataTable();
                // If the shipment is not existing, throw EntityNotFoundException
                //throw new EntityNotFoundException("Shipment with the specified identifier not found");

                // Add code to get the shipment details
                dt = PCT_EpicorDMT.PCT.GetShipmentDetail(salesforceOrderNumber);
                rowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(ConfigurationManager.AppSettings["LogFile"], true))
                    {
                        file.WriteLine("");
                        file.WriteLine("************ START " + DateTime.Now.ToString() + "*************");
                        shipment.EpicorOrderNumber = dt.Rows[0]["OrderNum"].ToString();
                        shipment.SalesForceOrderNumber = salesforceOrderNumber;
                        shipment.SalesForceAccountId = dt.Rows[0]["Salesforce_AcctID_c"].ToString();
                        shipment.PackingSlipNumber = dt.Rows[0]["PackNum"].ToString();
                        shipment.ShipmentDetails = new List<ShipmentDetail>();

                        int rowNumber = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            file.WriteLine("________________ Row " + rowNumber++ + " _______________________");
                            file.WriteLine("OrderNum:" + row["OrderNum"].ToString());
                            file.WriteLine("Salesforce_AcctID_c:" + row["Salesforce_AcctID_c"].ToString());
                            file.WriteLine("PackNum:" + row["PackNum"].ToString());
                            file.WriteLine("PartNum:" + row["PartNum"].ToString());
                            file.WriteLine("LineDesc:" + row["LineDesc"].ToString());
                            file.WriteLine("Qty:" + row["Qty"].ToString());
                            file.WriteLine("POLine:" + row["POLine"].ToString());
                            file.WriteLine("SN_IMEI_c (Comma Delimited):" + row["SN_IMEI_c"].ToString());

                            var shipmentDetail = new ShipmentDetail();
                            shipmentDetail.ProductCode = row["PartNum"].ToString();
                            shipmentDetail.ProductShortName = row["LineDesc"].ToString();
                            shipmentDetail.QuantityShipped = row["Qty"].ToString();
                            shipmentDetail.OrderItemNumber = row["POLine"].ToString();
                            shipmentDetail.IMEIList = new List<string>();

                            string[] sIMEI_List = row["SN_IMEI_c"].ToString().Split(';');
                            foreach (string sIMEI in sIMEI_List)
                            {
                                shipmentDetail.IMEIList.Add(sIMEI);
                            }
                            if (!String.IsNullOrEmpty(row["PartNum"].ToString()))
                            {
                                shipment.ShipmentDetails.Add(shipmentDetail);
                            }
                        }
                        file.WriteLine("");
                        file.WriteLine("END ");
                    }
                    
                }
          
            }
            catch (Exception ex)
            {
                // If there's a runtime exception, throw an InternalServerException
                throw new HttpResponseException(ResponseUtility.CreateHttpErrorResponse(new List<string> { ex.Message }, System.Net.HttpStatusCode.InternalServerError));
            }

            if (rowCount > 0)
            {
                return shipment;
            }
            else
            {
                throw new HttpResponseException(ResponseUtility.CreateHttpErrorResponse(new List<string> { "No record found for " + salesforceOrderNumber }, System.Net.HttpStatusCode.NotFound));

            }

        }

        /// <summary>
        ///     Updates order status.
        /// </summary>
        /// <param name="orderStatus"></param>
        public async Task<OrderStatusResponse> OrderStatus(OrderStatusRequest orderStatus)
        {
            // If the order is not existing, throw EntityNotFoundException
            //throw new EntityNotFoundException("Order with the specified identifier not found");
            try
            {
                // Add code to update the order status
                using (var httpClient = new HttpClient())
                {
                  
                    using (var response = await httpClient.PostAsJsonAsync(
                        ConfigurationManager.AppSettings["OrderStatusURL"], orderStatus))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var orderStatusResponse = JsonConvert.DeserializeObject<OrderStatusResponse>(apiResponse);
                        return orderStatusResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                // If there's a runtime exception, throw an InternalServerException
                throw new HttpResponseException(ResponseUtility.CreateHttpErrorResponse(new List<string> { ex.Message }, System.Net.HttpStatusCode.InternalServerError));
            }
        }
    }
}