﻿app.config(function ($stateProvider, $urlRouterProvider) {
    //$urlRouterProvider.when("", "");
    $stateProvider
       .state("searchPatient", {
           url: "/searchPatient",
           templateUrl: "../Home/SearchPatient"
       })

       .state("prescribeMCDT", {
           url: "/prescribeMCDT",
           templateUrl: "../MCDTs/PrescribeMCDT"
       })

         .state("regularExamsHistory", {
             url: "/RegularExamsHistory",
             templateUrl: "../RegularExamsHistory/GetRegularExamsHistory"
         })

       .state("classifyDisease", {
           url: "/classifyDisease",
           templateUrl: "../Diagnosis/ClassifyDisease_CID"
       })

       .state("updateDiseaseStatus", {
           url: "/UpdateDiseaseStatus",
           templateUrl: "../Diagnosis/UpdateDiseaseStatusResult"
       })

       .state("diagnosisHistory", {
           url: "/diagnosisHistory",
           templateUrl: "../Diagnosis/GetPatientDiagnosisHistory"
       })

        .state("prescribeMedication", {
            url: "/prescribeMedication",
            templateUrl: "../Medication/PrescribeMedication"
        })

         .state("medicationHistory", {
             url: "/medicationHistory",
             templateUrl: "../Medication/PrescribeMedicationHistory"
         })


    // ****************** Graphs ****************************//
    .state("mcdtResults", {
        url: "/MonitorizationGraphs",
        templateUrl: "../LabExams/MonitorizationGraphs"
    })

      .state("mcdtSpecificResults", {
          url: "/SpecificGraphMonitorization",
          templateUrl: "../LabExams/SpecificGraphMonitorization"
      })

    // **************** LabTec Template ********************//
    .state("addLabResults", {
        url: "/addLabResults",
        templateUrl: "../LabExams/ListMcdts",
    })

    //**************** Patient Info ************************//

    .state("addPatientInfo", {
        url: "/patientInfo",
        templateUrl: "../Patient/AddPatientInformation"
    })

    .state("consultPatientInfo", {
        url: "/consultPatientInfo",
        templateUrl: "../Patient/ListPatientInformation"
    })

    .state("clinicProfile", {
        url: "/clinicProfile",
        templateUrl: "../Staffs/ListClinicInformation"
    })

    .state("labTecProfile", {
        url: "/labTecProfile",
        templateUrl: "../Staffs/ListLabTecInformation"
    })

    //*********** Treatment Plan **************//
    .state("createTreatmentPlan", {
        url: "/createTreatmentPlan",
        templateUrl: "../TreatmentPlans/Index"
        //resolve: { title: 'TreatmentPlan' },
        //controller: function ($scope, title) {
        //    $scope.title = title;
        //},
        //onEnter: function (title) {
        //    if (title) { alert("enter"); }
        //},
        //onExit: function (title) {
        //    if (title) { alert("exits"); }
        //}
    })
    .state("consultTreatmentPlan", {
        url: "/TreatmentPlanMed",
        templateUrl: "../Content/TreatmentPlan/TreatmentPlanMed.html"
    })



    ;


});