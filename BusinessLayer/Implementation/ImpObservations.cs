﻿using BusinessLayer.Implementation.ViewModels;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.Entities.Visitas;
using DataLayer.Entities.VisitsEntities;
using DataLayer.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer.Implementation
{/// <summary>
/// This class implements IObservations interface
/// </summary>
    public class ImpObservations : IObservations
    {
        HPCareDBContext db;
      
        public ImpObservations(HPCareDBContext db)
        {
            this.db = db;

        }
        /// <summary>
        /// Save the received observation either from treatmentplan or observations panel
        /// </summary>
        /// <param name="observationList">includes observation body and subject</param>
        public void SaveObservation(List<string> observationList)
        {
            bool ISclassified = observationList[2] == "True" ? true : false;
            db.Observations.Add(new Observations
            {
                subject = observationList[0],
                ObservationBody = observationList[1],
                IsClassified = ISclassified,
                clinicalRegistry = SingletonClinicRegistry.GetInstance(db)
            });
            db.SaveChanges();
        }

        /// <summary>
        /// an uaxilliary function of which returns the registries associated to the user for the purpose
        /// of retrieving the associated observation
        /// </summary>
        /// <returns></returns>
        private List<ClinicRegistryManager> GetPatientClinicalRegisties()
        {
            Patient patient = db.Users.Find(HttpContext.Current.Session["patientId"]) as Patient;
            if (patient == null)
            {
                CurrentUserId currentStaff = new CurrentUserId();
                int currentIdStaff = currentStaff.AccessDatabase(HttpContext.Current.User.Identity.Name);

                patient = db.Users.Find(currentIdStaff) as Patient;
            }
          
            List<ClinicRegistryManager> clinicalRegistries =
                db.ClinicRegistryManagers.Where(x => x.Clinic_patient.User_id == patient.User_id).ToList();
            return clinicalRegistries;
        }
        /// <summary>
        /// Returns a list  of patient's observations view model
        /// </summary>
        /// <returns>List of observations view model</returns>
        public List<ObservationsVM> GetObservations()
        {
            List<ObservationsVM> listVm = new List<ObservationsVM>();
            foreach (ClinicRegistryManager reg in GetPatientClinicalRegisties())
            {

                listVm.AddRange(GetObservationVM(reg));

            }
            return listVm;
        }
        /// <summary>
        /// Created a observations viewmodel of the specified registry/patient
        /// 
        /// </summary>
        /// <param name="registry"></param>
        /// <returns>the observation viewmodel data-subject,body,date and author</returns>
        private List<ObservationsVM> GetObservationVM(ClinicRegistryManager registry)
        {
            ObservationsVM vm = null;
            List<ObservationsVM> listObservationsVM = new List<ObservationsVM>();
            VisitManager vManager = null;

            db.Entry(registry).Reference(x => x.Staff_doctor).Load();
            //List<Observations> obs = db.Observations.Where(x => x.clinicalRegistry.ClinicRegistryManagerId == registry.ClinicRegistryManagerId).ToList();
            List<Observations> obs = SetListObservation(registry);
            foreach (Observations item in obs)
            {
                vManager = db.VisitManagers.Where(x => x.PatientVisitRegistry.ClinicRegistryManagerId == registry.ClinicRegistryManagerId).FirstOrDefault();

                if (vManager != null)
                {
                    vm = new ObservationsVM();
                    vm.Subject = item.subject;
                    vm.observationID = item.observations_ID;
                    vm.Date = vManager.visit.Visit_Date;

                    vm.Author = db.ClinicRegistryManagers.Where(x => x.ClinicRegistryManagerId == registry.ClinicRegistryManagerId).FirstOrDefault().Staff_doctor.Name;
                }
                listObservationsVM.Add(vm);
            }

            return listObservationsVM;
        }
        private List<Observations> SetListObservation(ClinicRegistryManager registry)
        {
            Patient patient = db.Users.Find(HttpContext.Current.Session["patientId"]) as Patient;
           
            if (patient == null)
            {
                return
                     db.Observations.Where(x => x.clinicalRegistry.ClinicRegistryManagerId == registry.ClinicRegistryManagerId && x.IsClassified == false).ToList();
            }
            return db.Observations.Where(x => x.clinicalRegistry.ClinicRegistryManagerId == registry.ClinicRegistryManagerId).ToList();
        }

    }
}
