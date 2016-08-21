﻿using BusinessLayer.Interfaces;
using DataLayer.Entities.DiagnosisEntities;
using DataLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Entities.Visitas;
using System.Data.Entity.Infrastructure;
using BusinessLayer.Implementation;
using System.Web;

namespace BusinessLayer.Implementation
{
    public class impDiagnosis : IDiagnosis
    {

        private HPCareDBContext db = new HPCareDBContext();

        public string DeactivateDisease(Disease disease)
        {
            if (disease != null)
            {

                Disease diseaseUpdated = db.Diseases.Find(disease.Disease_id);
                diseaseUpdated.Disease_is_active = false;
                diseaseUpdated.Disease_end_date = DateTime.Now;
                db.Entry(diseaseUpdated).State = EntityState.Modified;
                db.SaveChanges();

                return "Disease Deactivated";

            }
            else
            {
                return "Invalid data";
            }
        }

        public List<Disease> getPatientActiveDiseases(Patient patient)
        {
            return db.Diseases.Where(x => x.Disease_is_active == true).ToList();
        }

        public List<Disease> getPatientInActiveDiseases(Patient patient)
        {
            return db.Diseases.Where(x => x.Disease_is_active == false).ToList();
        }
        public void SaveDiagnosis(List<CID_DiseaseCode> classifications)
        {
            foreach (CID_DiseaseCode diseaseCODE in classifications)
            {
                saveDiagnosisAUX(diseaseCODE);
            }
        }
        private void saveDiagnosisAUX(CID_DiseaseCode disCode)
        {
            CIDCode diseaseCode = new CIDCode { Version = "CID10", CID_DiseaseCode = disCode };
            int cidCodeID = db.CIDCodes.Add(diseaseCode).CIDCOD_id;
            Disease disease = new Disease { Disease_start_date = DateTime.Now, Disease_is_active = true };
            int diseaseID = db.Diseases.Add(disease).Disease_id;

            ClinicRegistryManager registry = SingletonClinicRegistry.GetInstance(db);
            try
            {
                Diagnosis diagnosis = new Diagnosis { Diagnosis_CID_code = diseaseCode, Diagnosis_disease = disease, ClinicRegistry_Manager = registry };
                diagnosis.Diagnosis_CID_code.CIDCOD_id = cidCodeID;
                diagnosis.Diagnosis_disease.Disease_id = diseaseID;
                db.Diagnoses.Add(diagnosis);
                db.SaveChanges();
                db.Entry(diagnosis).State = EntityState.Detached;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

    }
}