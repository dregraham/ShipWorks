using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Available Asendia service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AsendiaServiceType
    {
        [Description("Asendia Priority Tracked")]
        [ApiValue("asendia_priority_tracked")]
        [Deprecated]
        AsendiaPriorityTracked = 0,

        [Description("Asendia International Express")]
        [ApiValue("asendia_international_express")]
        [Deprecated]
        AsendiaInternationalExpress = 1,

        [Description("Asendia IPA")]
        [ApiValue("asendia_ipa")]
        [Deprecated]
        AsendiaIPA = 2,

        [Description("Asendia ISAL")]
        [ApiValue("asendia_isal")]
        [Deprecated]
        AsendiaISAL = 3,

        [Description("Asendia PMI")]
        [ApiValue("asendia_pmi")]
        [Deprecated]
        AsendiaPMI = 4,

        [Description("Asendia PMEI")]
        [ApiValue("asendia_pmei")]
        [Deprecated]
        AsendiaPMEI = 5,

        [Description("Asendia ePacket")]
        [ApiValue("asendia_epacket")]
        [Deprecated]
        AsendiaEPacket = 6,

        [Description("Asendia Other")]
        [ApiValue("asendia_other")]
        [Deprecated]
        AsendiaOther = 7,

        [Description("Business Mail Canada Lettermail Machineable")]
        [ApiValue("asendia_bus_mail_ca_lettermail_mac")]
        BusinessMailCanadaLettermailMachineable = 8,

        [Description("Business Mail Canada Lettermail")]
        [ApiValue("asendia_bus_mail_ca_lettermail")]
        BusinessMailCanadaLettermail = 9,

        [Description("Business Mail Economy LP Wholesale")]
        [ApiValue("asendia_bus_mail_eco_lp_wholesale")]
        BusinessMailEconomyLPWholesale = 10,

        [Description("Business Mail Economy SP Wholesale")]
        [ApiValue("asendia_bus_mail_eco_sp_wholesale")]
        BusinessMailEconomySPWholesale = 11,

        [Description("Business Mail Economy")]
        [ApiValue("asendia_bus_mail_eco")]
        BusinessMailEconomy = 12,

        [Description("Business Mail IPA")]
        [ApiValue("asendia_bus_mail_ipa")]
        BusinessMailIPA = 13,

        [Description("Business Mail ISAL")]
        [ApiValue("asendia_bus_mail_isal")]
        BusinessMailISAL = 14,

        [Description("Business Mail Priority LP Wholesale")]
        [ApiValue("asendia_bus_mail_pri_lp_wholesale")]
        BusinessMailPriorityLPWholesale = 15,

        [Description("Business Mail Priority SP Wholesale")]
        [ApiValue("asendia_pri_sp_wholesale")]
        BusinessMailPrioritySPWholesale = 16,

        [Description("Business Mail Priority")]
        [ApiValue("asendia_mail_pri")]
        BusinessMailPriority = 17,

        [Description("e-PAQ Elite Custom")]
        [ApiValue("asendia_epaq_elite_cus")]
        EpaqEliteCustom = 18,

        [Description("e-PAQ Elite DDP Oversized")]
        [ApiValue("asendia_epaq_elite_ddp_osd")]
        EpaqEliteDDPOversized = 19,

        [Description("e-PAQ Elite DDP")]
        [ApiValue("asendia_epaq_elite_ddp")]
        EpaqEliteDDP = 20,

        [Description("e-PAQ Elite Direct Access Canada DDP")]
        [ApiValue("asendia_epaq_elite_da_ca_ddp")]
        EpaqEliteDirectAccessCanadaDDP = 21,

        [Description("e-PAQ Elite DPD")]
        [ApiValue("asendia_epaq_elite_dpd")]
        EpaqEliteDPD = 22,

        [Description("e-PAQ Elite Oversized")]
        [ApiValue("asendia_epaq_elite_osd")]
        EpaqEliteOversized = 23,

        [Description("e-PAQ Elite")]
        [ApiValue("asendia_epaq_elite")]
        EpaqElite = 24,

        [Description("e-PAQ Plus Custom")]
        [ApiValue("asendia_epaq_pls_custom")]
        EpaqPlusCustom = 25,

        [Description("e-PAQ Plus DAP")]
        [ApiValue("asendia_epaq_plus_dap")]
        EpaqPlusDAP = 26,

        [Description("e-PAQ Plus DDP")]
        [ApiValue("asendia_epaq_plus_ddp")]
        EpaqPlusDDP = 27,

        [Description("e-PAQ Plus Economy")]
        [ApiValue("asendia_epaq_plus_eco")]
        EpaqPlusEconomy = 28,

        [Description("e-PAQ Plus ePacket Canada Customs PrePaid")]
        [ApiValue("asendia_epaq_plus_ca_cp")]
        EpaqPlusePacketCanadaCustomsPrePaid = 29,

        [Description("e-PAQ Plus ePacket Canada DDP")]
        [ApiValue("asendia_epaq_plus_ca_ddp")]
        EpaqPlusePacketCanadaDDP = 30,

        [Description("e-PAQ Plus ePacket")]
        [ApiValue("asendia_epaq_plus_epacket")]
        EpaqPlusePacket = 31,

        [Description("e-PAQ Plus Wholesale")]
        [ApiValue("asendia_epaq_plus_wholesale")]
        EpaqPlusWholesale = 32,

        [Description("e-PAQ Plus")]
        [ApiValue("asendia_epaq_plus")]
        EpaqPlus = 33,

        [Description("e-PAQ Returns International")]
        [ApiValue("asendia_epaq_ret_int")]
        EpaqReturnsInternational = 34,

        [Description("e-PAQ Select Custom")]
        [ApiValue("asendia_epaq_select_custom")]
        EpaqSelectCustom = 35,

        [Description("e-PAQ Select DDP Direct Access")]
        [ApiValue("asendia_epaq_sel_ddp_diracc")]
        EpaqSelectDDPDirectAccess = 36,

        [Description("e-PAQ Select DDP")]
        [ApiValue("asendia_epaq_select_ddp")]
        EpaqSelectDDP = 37,

        [Description("e-PAQ Select Direct Access Canada DDP")]
        [ApiValue("asendia_epaq_sel_directaccess_ca_ddp")]
        EpaqSelectDirectAccessCanadaDDP = 38,

        [Description("e-PAQ Select Direct Access")]
        [ApiValue("asendia_epaq_sel_directaccess")]
        EpaqSelectDirectAccess = 39,

        [Description("e-PAQ Select Economy")]
        [ApiValue("asendia_epaq_sel_eco")]
        EpaqSelectEconomy = 40,

        [Description("e-PAQ Select Oversized DDP")]
        [ApiValue("asendia_epaq_sel_oversized_ddp")]
        EpaqSelectOversizedDDP = 41,

        [Description("e-PAQ Select Oversized")]
        [ApiValue("asendia_epaq_sel_oversized")]
        EpaqSelectOversized = 42,

        [Description("e-Paq Select PMEI Non Presort")]
        [ApiValue("asendia_epaq_sel_pmei_nonpresort")]
        EpaqSelectPMEINonPresort = 43,

        [Description("e-PAQ Select PMEI PC Postage")]
        [ApiValue("asendia_epaq_sel_pmei_pc_postage")]
        EpaqSelectPMEIPCPostage = 44,

        [Description("e-PAQ Select PMEI")]
        [ApiValue("asendia_epaq_sel_pmei")]
        EpaqSelectPMEI = 45,

        [Description("e-PAQ Select PMI Canada Customs Prepaid")]
        [ApiValue("asendia_epaq_sel_pmi_ca_cust_prepaid")]
        EpaqSelectPMICanadaCustomsPrepaid = 46,

        [Description("e-PAQ Select PMI Canada DDP")]
        [ApiValue("asendia_epaq_sel_pmi_ca_ddp")]
        EpaqSelectPMICanadaDDP = 47,

        [Description("e-PAQ Select PMI Non Presort")]
        [ApiValue("asendia_epaq_sel_pmi_nonpresort")]
        EpaqSelectPMINonPresort = 48,

        [Description("e-PAQ Select PMI PC Postage")]
        [ApiValue("asendia_epaq_sel_pmi_pc_postage")]
        EpaqSelectPMIPCPostage = 49,

        [Description("e-PAQ Select PMI")]
        [ApiValue("asendia_epaq_select_pmi")]
        EpaqSelectPMI = 50,

        [Description("e-PAQ Select")]
        [ApiValue("asendia_epaq_sel")]
        EpaqSelect = 51,

        [Description("e-PAQ Standard Custom")]
        [ApiValue("asendia_epaq_stand_cus")]
        EpaqStandardCustom = 52,

        [Description("e-PAQ Standard IPA")]
        [ApiValue("asendia_epaq_stand_ipa")]
        EpaqStandardIPA = 53,

        [Description("e-PAQ Standard ISAL")]
        [ApiValue("asendia_epaq_stand_isal")]
        EpaqStandardISAL = 54,

        [Description("e-PAQ Standard")]
        [ApiValue("asendia_epaq_stand")]
        EpaqStandard = 55,

        [Description("Marketing Mail Canada Personalized LCP")]
        [ApiValue("asendia_mark_mail_ca_per_lcp")]
        MarketingMailCanadaPersonalizedLCP = 56,

        [Description("Marketing Mail Canada Personalized Machineable")]
        [ApiValue("asendia_mark_mail_ca_per_mach")]
        MarketingMailCanadaPersonalizedMachineable = 57,

        [Description("Marketing Mail Canada Personalized NDG")]
        [ApiValue("asendia_mark_mail_ca_per_ndg")]
        MarketingMailCanadaPersonalizedNDG = 58,

        [Description("Marketing Mail Economy")]
        [ApiValue("asendia_mark_mail_eco")]
        MarketingMailEconomy = 59,

        [Description("Marketing Mail IPA")]
        [ApiValue("asendia_mark_mail_ipa")]
        MarketingMailIPA = 60,

        [Description("Marketing Mail ISAL")]
        [ApiValue("asendia_mark_mail_isal")]
        MarketingMailISAL = 61,

        [Description("Marketing Mail Priority")]
        [ApiValue("asendia_mark_mail_pri")]
        MarketingMailPriority = 62,

        [Description("Publications Canada LCP")]
        [ApiValue("asendia_pub_ca_lcp")]
        PublicationsCanadaLCP = 63,

        [Description("Publications Canada NDG")]
        [ApiValue("asendia_pub_ca_ndg")]
        PublicationsCanadaNDG = 64,

        [Description("Publications Economy")]
        [ApiValue("asendia_pub_eco")]
        PublicationsEconomy = 65,

        [Description("Publications IPA")]
        [ApiValue("asendia_pub_ipa")]
        PublicationsIPA = 66,

        [Description("Publications ISAL")]
        [ApiValue("asendia_pub_isal")]
        PublicationsISAL = 67,

        [Description("Publications Priority")]
        [ApiValue("asendia_pub_pri")]
        PublicationsPriority = 68,

        [Description("e-PAQ Select DG")]
        [ApiValue("asendia_epaq_sel_dg")]
        EpaqSelectDG = 69,

        [Description("e-PAQ Select Direct Access Canada DDP Undeliverable")]
        [ApiValue("asendia_epaq_sel_da_ca_ddp_undeliverable")]
        EpaqSelectDirectAccessCanadaDDPUndeliverable = 70,

        [Description("e-PAQ Select Direct Access Returns")]
        [ApiValue("asendia_epaq_sel_da_access_ret")]
        EpaqSelectDirectAccessReturns = 71,

        [Description("e-PAQ Select Direct Access Undeliverable")]
        [ApiValue("asendia_epaq_sel_da_access_undeliverable")]
        EpaqSelectDirectAccessUndeliverable = 72,

        [Description("e-PAQ Returns Internal")]
        [ApiValue("asendia_epaq_ret_internal")]
        EpaqReturnsInternal = 73,

        [Description("e-PAQ Elite Direct Access Canada DDP Returns")]
        [ApiValue("asendia_epaq_elite_da_ca_ddp_ret")]
        EpaqEliteDirectAccessCanadaDDPReturns = 74,

        [Description("e-PAQ Elite Direct Access Canada DDP Undeliverable")]
        [ApiValue("asendia_epaq_elite_da_ca_ddp_undel")]
        EpaqEliteDirectAccessCanadaDDPUndeliverable = 75,

        [Description("e-PAQ Select Canada Fulfillment")]
        [ApiValue("asendia_epaq_sel_ca_fullfillment")]
        EpaqSelectCanadaFulfillment = 76,

        [Description("e-PAQ Select DDP DG")]
        [ApiValue("asendia_epaq_sel_ddp_dg")]
        EpaqSelectDDPDG = 77,

        [Description("e-PAQ Select DDP Direct Access Returns")]
        [ApiValue("asendia_epaq_sel_ddp_direct_acc_ret")]
        EpaqSelectDDPDirectAccessReturns = 78,

        [Description("e-PAQ Select DDP Direct Access Undeliverable")]
        [ApiValue("asendia_epaq_sel_ddp_direct_acc_undeliverable")]
        EpaqSelectDDPDirectAccessUndeliverable = 79,

        [Description("e-PAQ Select Direct Access Canada DDP Returns")]
        [ApiValue("asendia_epaq_sel_da_dir_acc_ca_ddp_ret")]
        EpaqSelectDirectAccessCanadaDDPReturns = 80,

        [Description("e-PAQ Select Direct Access Intra Canada DDP")]
        [ApiValue("asendia_epaq_sel_dir_acc_intra_ca_ddp")]
        EpaqSelectDirectAccessIntraCanadaDDP = 81,

        [Description("e-PAQ Select Mailbox Plus DDP")]
        [ApiValue("asendia_epaq_sel_mailbox_plus_ddp")]
        EpaqSelectMailboxPlusDDP = 82,

        [Description("e-PAQ Select Pre Processed Canada DDP")]
        [ApiValue("asendia_epaq_sel_preprocess_ca_ddp")]
        EpaqSelectPreProcessedCanadaDDP = 83,

        [Description("e-PAQ Select Reverse Direct To Export")]
        [ApiValue("asendia_epaq_sel_reverse_direct_export")]
        EpaqSelectReverseDirectToExport = 84,

        [Description("e-PAQ Elite DDP eSW Fluency")]
        [ApiValue("asendia_epaq_elite_ddp_esw_flu")]
        EpaqEliteDDPeSWFluency = 85,

        [Description("e-PAQ Elite DDP eSW Fluency DG")]
        [ApiValue("asendia_epaq_elite_ddp_esw_flu_dg")]
        EpaqEliteDDPeSWFluencyDG = 86,

        [Description("e-PAQ Select Customs Prepaid By Shopper")]
        [ApiValue("asendia_sel_cust_pre_shopper")]
        EpaqSelectCustomsPrepaidByShopper = 87,

        [Description("e-PAQ Select PMEI Customs Prepaid")]
        [ApiValue("asendia_sel_pmei_cus_prepaid")]
        EpaqSelectPMEICustomsPrepaid = 88,

        [Description("e-PAQ Standard Books")]
        [ApiValue("asendia_epaq_stand_books")]
        EpaqStandardBooks = 89
    }
}
